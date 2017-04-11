using Quantium.Recruitment.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using AutoMapper;
using Quantium.Recruitment.Models;
using Quantium.Recruitment.Entities;
using Quantium.Recruitment.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using AspNetCoreSpa.Server.Repositories.Abstract;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Threading;
using AspNetCoreSpa.Server;
using Microsoft.AspNetCore.Authorization;

namespace Quantium.Recruitment.ApiServices.Controllers
{
    [Authorize(Roles = "Admin, Candidate")]
    [Route("[controller]/[action]/{id?}")]
    public class ChallengeController : Controller
    {
        private readonly IEntityBaseRepository<Job> _jobRepository;
        private readonly IEntityBaseRepository<Candidate> _candidateRepository;
        private readonly IEntityBaseRepository<Candidate_Job> _candidateJobRepository;
        private readonly IEntityBaseRepository<Test> _testRepository;
        private readonly IEntityBaseRepository<Job_Difficulty_Label> _jobDifficultyLabelRepository;
        private readonly IEntityBaseRepository<Question> _questionRepository;
        private readonly IEntityBaseRepository<Challenge> _challengeRepository;
        private readonly IEntityBaseRepository<Option> _optionRepository;
        private readonly IServiceProvider _serviceProvider;

        private CancellationTokenSource source;

        public ChallengeController(
            IEntityBaseRepository<Job> jobRepository,
            IEntityBaseRepository<Candidate> candidateRepository,
            IEntityBaseRepository<Candidate_Job> candidateJobRepository,
            IEntityBaseRepository<Test> testRepository,
            IEntityBaseRepository<Job_Difficulty_Label> jobDifficultyLabelRepository,
            IEntityBaseRepository<Question> questionRepository,
            IEntityBaseRepository<Challenge> challengeRepository,
            IEntityBaseRepository<Option> optionRepository,
            IServiceProvider serviceProvider)
        {
            _jobRepository = jobRepository;
            _candidateRepository = candidateRepository;
            _candidateJobRepository = candidateJobRepository;
            _testRepository = testRepository;
            _jobDifficultyLabelRepository = jobDifficultyLabelRepository;
            _questionRepository = questionRepository;
            _challengeRepository = challengeRepository;
            _optionRepository = optionRepository;
            _serviceProvider = serviceProvider;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var email = this.User.Identities.First().Name;

                var test = _testRepository.FindBy(t => t.Candidate.Email == email).FirstOrDefault();

                if (test.IsFinished == true)
                {
                    return Ok(JsonConvert.SerializeObject("Finished"));
                }

                if (test.Challenges == null)
                {
                    test.IsFinished = true;
                    _testRepository.Edit(test);
                    _testRepository.Commit();
                    return Ok(JsonConvert.SerializeObject("Finished"));
                }

                var challenges = test.Challenges.Where(c => c.TestId == test.Id).OrderBy(c => c.Id).ToList();

                var totalCount = challenges.Count;
                var notSentChallenges = challenges.Where(c => c.IsSent != true);

                bool[] totalChallengesAnswered = new bool[challenges.Count];

                for (int i = 0; i < challenges.Count; i++)
                {
                    totalChallengesAnswered[i] = (challenges[i].IsAnswered == null || challenges[i].IsAnswered == false) ? false : true;
                }

                var currentChallenge = notSentChallenges.Count() > 0 ? notSentChallenges.OrderBy(c => c.Id).First() : null;

                if (currentChallenge == null)
                {
                    test.IsFinished = true;
                    test.FinishedDate = DateTime.Today.Date;
                    _testRepository.Edit(test);
                    _testRepository.Commit();
                    return Ok(JsonConvert.SerializeObject("Finished"));
                }

                var currentChallengeDto = Mapper.Map<ChallengeDto>(currentChallenge);

                currentChallengeDto.RemainingChallenges = notSentChallenges.Count() - 1;
                currentChallengeDto.currentChallenge = totalCount - notSentChallenges.Count() + 1;
                currentChallengeDto.ChallengesAnswered = totalChallengesAnswered;

                double totalTestTimeInMins = ((challenges.Sum(c => c.Question.TimeInSeconds)) / 60.0);
                double remainingTestTimeInMins = ((notSentChallenges.Sum(c => c.Question.TimeInSeconds)) / 60.0) - (currentChallengeDto.Question.TimeInSeconds / 60.0);
                currentChallengeDto.TotalTestTimeInMinutes = totalTestTimeInMins.ToString();
                currentChallengeDto.RemainingTestTimeInMinutes = remainingTestTimeInMins.ToString();
                currentChallengeDto.Question.Options.ForEach(o => o.IsAnswer = false);

                if (currentChallengeDto.Question.RandomizeOptions)
                {
                    currentChallengeDto.Question.Options = currentChallengeDto.Question.Options.OrderBy(item => Guid.NewGuid()).ToList();
                }

                if (currentChallengeDto.RemainingChallenges == 0)
                {
                    test.IsFinished = true;
                    test.FinishedDate = DateTime.UtcNow;
                    _testRepository.Edit(test);
                    _testRepository.Commit();
                }

                currentChallenge.StartTime = currentChallenge.StartTime == null ? DateTime.Now : currentChallenge.StartTime;
                var challengeStartTime = currentChallenge.StartTime.Value;
                var elapsedTime = DateTime.Now.Subtract(challengeStartTime).Seconds;
                var calculatedTime = currentChallengeDto.Question.TimeInSeconds - elapsedTime;
                if (calculatedTime > 0)
                {
                    currentChallengeDto.Question.TimeInSeconds = currentChallengeDto.Question.TimeInSeconds - elapsedTime;
                }
                else
                {
                    currentChallengeDto.Question.TimeInSeconds = 0;
                }

                _challengeRepository.Edit(currentChallenge);
                _challengeRepository.Commit();

                await Task.Run(() => RunTimer(currentChallengeDto));

                return Ok(currentChallengeDto);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, JsonConvert.SerializeObject(ex.Message + ex.InnerException.Message));
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody]ChallengeDto challengeDto)
        {
            try
            {
                var challenge = _challengeRepository.GetAll().SingleOrDefault(c => c.IsSent != true && c.Id == challengeDto.Id);
                if (challenge == null)
                    return Ok(JsonConvert.SerializeObject("Answer received too late"));

                challenge.IsSent = true;
                challenge.IsAnswered = challengeDto.CandidateSelectedOptions.Count > 0 ? true : false;
                challenge.StartTime = challengeDto.StartTime;
                challenge.AnsweredTime = challengeDto.AnsweredTime;
                challenge.CandidateSelectedOptions = Mapper.Map<List<CandidateSelectedOption>>(challengeDto.CandidateSelectedOptions);

                foreach (var item in challenge.CandidateSelectedOptions)
                {
                    item.Challenge = challenge;
                    item.Option = _optionRepository.GetSingle(item.OptionId);
                }

                _challengeRepository.Edit(challenge);
                _challengeRepository.Commit();
                return Ok(JsonConvert.SerializeObject("Success"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, JsonConvert.SerializeObject(ex.Message + ex.InnerException.Message));
            }
        }

        [HttpPost]
        public IActionResult FinishAllChallenges([FromBody]long testId)
        {
            try
            {
                var test = _testRepository.GetSingle(testId);
                test.IsFinished = true;
                test.FinishedDate = DateTime.UtcNow;
                _testRepository.Edit(test);
                _testRepository.Commit();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

            return Ok(JsonConvert.SerializeObject("Finished"));
        }

        [HttpGet]
        public async Task<IActionResult> IsTestFinished()
        {
            var email = this.User.Identities.First().Name;

            var testsWithEmail = await _testRepository.FindByAsync(t => t.Candidate.Email == email);

            var test = testsWithEmail.FirstOrDefault();

            var isFinished = test != null ? test.IsFinished : false;

            return Ok(JsonConvert.SerializeObject(isFinished));
        }

        [HttpGet]
        public async Task<IActionResult> IsTestAssigned()
        {
            var email = this.User.Identities.First().Name;

            var testsWithEmail = await _testRepository.FindByAsync(t => t.Candidate.Email == email);

            var test = testsWithEmail.FirstOrDefault();

            var isAssigned = test != null ? true : false;

            return Ok(JsonConvert.SerializeObject(isAssigned));
        }

        private async void RunTimer(ChallengeDto currentChallengeDto)
        {
            this.source = new CancellationTokenSource();
            //source.CancelAfter(TimeSpan.FromSeconds(currentChallengeDto.Question.TimeInSeconds));
            await Task.Delay((currentChallengeDto.Question.TimeInSeconds * 1000) + 8000); //8 buffer seconds for latency
            Task<bool> task = Task.Run(() => UpdateQuestionAfterTimer(currentChallengeDto, source.Token), source.Token);
        }

        private bool UpdateQuestionAfterTimer(ChallengeDto currentChallengeDto, CancellationToken cancellationToken)
        {
            var currentChallenge = _challengeRepository.GetSingleUsingNewContext(currentChallengeDto.Id);

            if (currentChallenge.IsSent != true)
            {
                currentChallenge.IsSent = true;
                _challengeRepository.UpdateWithNewContext(currentChallenge);
            }

            return true;
        }
    }
}