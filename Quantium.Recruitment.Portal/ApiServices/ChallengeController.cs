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
using System.Collections;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNetCore.Mvc;

namespace Quantium.Recruitment.ApiServices.Controllers
{
    //[Route("api/challenge")]
    [Route("api/[controller]/[action]/{id?}")]
    public class ChallengeController : Controller
    {
        private readonly IChallengeRepository _challengeRepository;
        private readonly ITestRepository _testRepository;
        private readonly ICandidateRepository _candidateRepository;
        private readonly IOptionRepository _optionRepository;
        private readonly ICandidateSelectedOptionRepository _candidateSelectedOptionRepository;
        private CancellationTokenSource source;

        public ChallengeController(
            IChallengeRepository challengeRepository,
            ITestRepository testRepository,
            ICandidateSelectedOptionRepository candidateSelectedOptionRepository,
            ICandidateRepository candidateRepository,
            IOptionRepository optionRepository)
        {
            _challengeRepository = challengeRepository;
            _testRepository = testRepository;
            _candidateSelectedOptionRepository = candidateSelectedOptionRepository;
            _candidateRepository = candidateRepository;
            _optionRepository = optionRepository;
        }

        [HttpGet]
        public IActionResult GetAllChallenges()
        {
            var challenges = _challengeRepository.GetAll().ToList();

            var cDtos = Mapper.Map<List<ChallengeDto>>(challenges);

            return Ok(cDtos);
        }

        [HttpGet]
        public async Task<IActionResult> GetNext(string email)
        {
            var test = _testRepository.FindByCandidateEmail(email);

            if(test.IsFinished == true)
            {
                return Ok("Finished");
            }

            if(test.Challenges == null)
            {
                test.IsFinished = true;
                _testRepository.Update(test);
                return Ok("Finished");
            }

            var challenges = test.Challenges.Where(c => c.TestId == test.Id).OrderBy(c => c.Id).ToList();

            var totalCount = challenges.Count;
            var notSentChallenges = challenges.Where(c => c.IsSent != true);

            bool[] totalChallengesAnswered = new bool[challenges.Count];

            for (int i = 0; i < challenges.Count; i++)
            {
                totalChallengesAnswered[i] = (challenges[i].IsAnswered == null || challenges[i].IsAnswered == false) ? false: true;
            }

            var currentChallenge = notSentChallenges.Count() > 0 ? notSentChallenges.OrderBy(c => c.Id).First() : null;

            if(currentChallenge == null)
            {
                test.IsFinished = true;
                test.FinishedDate = DateTime.Today.Date;
                _testRepository.Update(test);
                return Ok("Finished");
            }

            var currentChallengeDto = Mapper.Map<ChallengeDto>(currentChallenge);

            currentChallengeDto.RemainingChallenges = notSentChallenges.Count() - 1;
            currentChallengeDto.currentChallenge = totalCount - notSentChallenges.Count() + 1;
            currentChallengeDto.ChallengesAnswered = totalChallengesAnswered;
            currentChallengeDto.Question.Options.ForEach(o => o.IsAnswer = false);

            if (currentChallengeDto.RemainingChallenges == 0)
            {
                test.IsFinished = true;
                test.FinishedDate = DateTime.UtcNow;
                _testRepository.Update(test);
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

            _challengeRepository.Update(currentChallenge);

            await Task.Run(() => RunTimer(currentChallengeDto));

            return Ok(currentChallengeDto);

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
            var currentChallenge =_challengeRepository.FindByIdUsingNewContext(currentChallengeDto.Id);

            if (currentChallenge.IsSent != true)
            {
                currentChallenge.IsSent = true;
                _challengeRepository.Update(currentChallenge);
            }

            return true;
        }

        [HttpPost]
        public IActionResult PostChallenge([FromBody]ChallengeDto challengeDto)
        {
            //var challenge = Mapper.Map<Challenge>(challengeDto);

            var challenge = _challengeRepository.GetAll().SingleOrDefault(c => c.IsSent != true && c.Id == challengeDto.Id);
            if (challenge == null)
                return Ok("Answer received too late");
            challenge.IsSent = true;
            challenge.IsAnswered = challengeDto.CandidateSelectedOptions.Count > 0 ? true : false;
            challenge.StartTime = challengeDto.StartTime;
            challenge.AnsweredTime = challengeDto.AnsweredTime;
            challenge.CandidateSelectedOptions = Mapper.Map<List<CandidateSelectedOption>>(challengeDto.CandidateSelectedOptions);
            //challenge.CandidateSelectedOptions.ForEach(item => item.Challenge = challenge);

            foreach (var item in challenge.CandidateSelectedOptions)
            {
                item.Challenge = challenge;
                item.Option = _optionRepository.FindById(item.OptionId);
            }
            //challenge.Question.Challenges = new List<Challenge>();
            //challenge.Question.Challenges.Add(challenge);

            _challengeRepository.Update(challenge);
            return Ok("success");
        }
    }
}