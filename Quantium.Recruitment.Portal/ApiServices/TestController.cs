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
using Microsoft.AspNetCore.Http;

namespace Quantium.Recruitment.ApiServices.Controllers
{
    //[Route("api/test")]
    [Route("api/[controller]/[action]/{id?}")]
    public class TestController : Controller
    {
        private readonly ITestRepository _testRepository;
        private readonly IJobRepository _jobRepository;
        private readonly ICandidateRepository _candidateRepository;
        private readonly ICandidateJobRepository _candidateJobRepository;
        private readonly IJobLabelDifficultyRepository _jobDifficultyLabelRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IChallengeRepository _challengeRepository;

        public TestController(
            ITestRepository testRepository, 
            IJobRepository jobRepository,
            ICandidateRepository candidateRepository,
            ICandidateJobRepository candidateJobRepository,
            IJobLabelDifficultyRepository jobDifficultyLabelRepository,
            IQuestionRepository questionRepository,
            IChallengeRepository challengeRepository)
        {
            _testRepository = testRepository;
            _jobRepository = jobRepository;
            _candidateRepository = candidateRepository;
            _candidateJobRepository = candidateJobRepository;
            _jobDifficultyLabelRepository = jobDifficultyLabelRepository;
            _questionRepository = questionRepository;
            _challengeRepository = challengeRepository;
        }

        [HttpPost]
        public IActionResult GenerateTests([FromBody]List<Candidate_JobDto> candidatesJobsDto)
        {
            try
            {
                var candidatesJobs = Mapper.Map<List<Candidate_Job>>(candidatesJobsDto);
                var job = _jobRepository.FindById(candidatesJobsDto.First().Job.Id);

                foreach (var candidateJobDto in candidatesJobsDto)
                {
                    var candidate = _candidateRepository.FindById(candidateJobDto.Candidate.Id);

                    var newCandidateJob = new Candidate_Job
                    {
                        Job = job,
                        Candidate = candidate
                    };

                    var candidateJob = candidate.CandidateJobs.FirstOrDefault(cj => cj.CandidateId == candidate.Id && cj.JobId == job.Id);

                    if (candidateJob == null)
                        _candidateJobRepository.Add(newCandidateJob);

                    Test newTest = new Test
                    {
                        Name = job.Title + candidate.FirstName,
                        Candidate = candidate,
                        Job = job,
                        CreatedUtc = DateTime.UtcNow
                    };

                    var activeTest = candidate.Tests.FirstOrDefault(t => t.IsFinished != true && t.Job.Id == job.Id);

                    if (activeTest == null)
                    {
                        _testRepository.Add(newTest);
                    }
                    else
                    {
                        continue;
                    }

                    IList<Job_Difficulty_Label> jobDifficultyLabels = _jobDifficultyLabelRepository.FindByJobId(job.Id).ToList();

                    List<Question> selectedQuestions = new List<Question>();

                    foreach (var jobDiffLabel in jobDifficultyLabels)
                    {
                        var questions =
                            _questionRepository
                            .GetAll()
                            .Where(ques => ques.DifficultyId == jobDiffLabel.Difficulty.Id && ques.Label.Id == jobDiffLabel.Label.Id).ToList();

                        var availableQuestionCount = questions.Count();

                        if (availableQuestionCount < jobDiffLabel.DisplayQuestionCount)
                        {
                            throw new Exception("Question count exceeds available questions count");
                        }

                        var randomQuestions = questions.OrderBy(item => Guid.NewGuid()).Take(jobDiffLabel.DisplayQuestionCount);

                        selectedQuestions.AddRange(randomQuestions);
                    }

                    foreach (var question in selectedQuestions)
                    {
                        Challenge newChallenge = new Challenge
                        {
                            Test = activeTest == null ? newTest : activeTest,
                            Question = question
                        };

                        _challengeRepository.Add(newChallenge);
                    }
                }
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpGet]
        public IActionResult HasActiveTestForCandidate(string email)
        {
            var activeTest = _testRepository.FindActiveTestByCandidateEmail(email);

            if (activeTest == null)
                return Ok(false);
            else
                return Ok(true);
        }

        [HttpPost]
        public IActionResult ArchiveTests([FromBody]long[] testIds)
        {
            try
            {
                foreach (var testId in testIds)
                {
                    var test = _testRepository.FindById(testId);
                    test.IsArchived = true;
                    _testRepository.Update(test);
                }

            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok();
        }

        [HttpGet]
        public IActionResult GetFinishedTests()
        {
            var finishedTests = _testRepository.GetAll().Where(t => t.IsFinished == true && t.IsArchived != true).OrderByDescending(t => t.FinishedDate).ToList();
            var finishedTestDtos = Mapper.Map<List<TestDto>>(finishedTests);
            finishedTestDtos.ForEach(finishedTestDto =>
            {
                var finishedTest = finishedTests.SingleOrDefault(t => t.Id == finishedTestDto.Id);
                FillTestDto(finishedTest, finishedTestDto);
            });

            return Ok(finishedTestDtos);
        }

        [HttpGet]
        public IActionResult GetAllTests()
        {
            var allTests = _testRepository.GetAll().Where(t => t.IsArchived != true).OrderByDescending(t => t.FinishedDate).ToList();
            var allTestDtos = Mapper.Map<List<TestDto>>(allTests);

            return Ok(allTestDtos);
        }

        private TestDto FillTestDto(Test finishedTest, TestDto finishedTestDto)
        {
            finishedTestDto.TotalChallengesDisplayed = finishedTestDto.Challenges.Count;
            var answeredChallenges = finishedTest.Challenges.Where(c => c.IsAnswered == true);
            var jobDiffLabels = Mapper.Map<List<Job_Difficulty_LabelDto>>(finishedTest.Job.JobDifficultyLabels);

            var twoKeyJobDiffLabelMap = jobDiffLabels.Select(jdl => new Job_Difficulty_LabelDto
            {
                Label = jdl.Label,
                Difficulty = jdl.Difficulty,
                PassingQuestionCount = jdl.PassingQuestionCount,
                AnsweredCount = 0
            }).ToList();

            int totalRightAnswers = 0;
            finishedTestDto.TotalChallengesAnswered = answeredChallenges.Count();

            foreach (var answeredChallenge in answeredChallenges)
            {
                var questionLabel = answeredChallenge.Question.Label;
                var questionDifficulty = answeredChallenge.Question.Difficulty;

                var answersIds = answeredChallenge.Question.Options.Where(o => o.IsAnswer == true).Select(o => o.Id);
                var candidateAnswersIds = answeredChallenge.CandidateSelectedOptions.Select(cso => cso.OptionId);

                if (answersIds.Intersect(candidateAnswersIds).Count() == answersIds.Count() && answersIds.Count() == candidateAnswersIds.Count())
                {
                    totalRightAnswers += 1;
                    var jobDiffLabel =
                        twoKeyJobDiffLabelMap.Single(
                          item =>
                              item.Difficulty.Id == answeredChallenge.Question.DifficultyId &&
                              item.Label.Id == answeredChallenge.Question.LabelId);

                    string labelName = jobDiffLabel.Label.Name;

                    jobDiffLabel.AnsweredCount += 1;

                    }

                var optionDtos = finishedTestDto.Challenges.Single(c => c.Id == answeredChallenge.Id).Question.Options;

                foreach (var option in answeredChallenge.Question.Options)
                {
                    if (option.IsAnswer == true && candidateAnswersIds.Contains(option.Id))
                    {
                        optionDtos.Single(o => o.Id == option.Id).IsCandidateSelected = true;
                    }
                }
            }

            finishedTestDto.TotalRightAnswers = totalRightAnswers;
            finishedTestDto.IsTestPassed = true;

            foreach (var item in twoKeyJobDiffLabelMap)
            {
                if (item.PassingQuestionCount > item.AnsweredCount)
                {
                    finishedTestDto.IsTestPassed = false;
                    break;
                }
            }

            return finishedTestDto;
        }


        private void IsTestPassed(Test finishedTest, TestDto finishedTestDto)
        {
            var jobDiffLabels = Mapper.Map<List<Job_Difficulty_LabelDto>>(finishedTest.Job.JobDifficultyLabels);

            var answeredChallenges = finishedTest.Challenges.Where(c => c.IsAnswered == true);

            int totalRightAnswers = 0;

            var twoKeyJobDiffLabelMap = jobDiffLabels.Select(jdl => new Job_Difficulty_LabelDto
            {
                Label = jdl.Label,
                Difficulty = jdl.Difficulty,
                PassingQuestionCount = jdl.PassingQuestionCount,
                AnsweredCount = 0
            });

            foreach (var answeredChallenge in answeredChallenges)
            {
                var questionLabel = answeredChallenge.Question.Label;
                var questionDifficulty = answeredChallenge.Question.Difficulty;

                var answersIds = answeredChallenge.Question.Options.Where(o => o.IsAnswer == true).Select(o => o.Id);
                var candidateAnswersIds = answeredChallenge.CandidateSelectedOptions.Select(cso => cso.OptionId);

                if (answersIds.Intersect(candidateAnswersIds).Count() == answersIds.Count() && answersIds.Count() == candidateAnswersIds.Count())
                {
                    totalRightAnswers += 1;
                    twoKeyJobDiffLabelMap.Single(
                        item =>
                            item.Difficulty.Id == answeredChallenge.Question.DifficultyId &&
                            item.Label.Id == answeredChallenge.Question.LabelId).AnsweredCount += 1;
                }
            }

            

            
        }

        [HttpGet]
        public IActionResult GetTestById(long id)
        {
            var testDto = Mapper.Map<TestDto>(_testRepository.GetAll().SingleOrDefault(t => t.Id == id));
            return Ok(testDto);
        }

        [HttpGet]
        public IActionResult GetFinishedTestById(long id)
        {
            var finishedTest = _testRepository.GetAll().SingleOrDefault(t => t.Id == id && t.IsFinished == true);

            if (finishedTest == null)
                throw new Exception("Finished test not found");

            var finishedTestDto = Mapper.Map<TestDto>(finishedTest);
            FillTestDto(finishedTest, finishedTestDto);

            return Ok(finishedTestDto);
        }

        [HttpPost]
        public IActionResult FinishTest([FromBody]long id)
        {
            try
            {
                var test = _testRepository.FindById(id);
                test.IsFinished = true;
                test.FinishedDate = DateTime.UtcNow;
                _testRepository.Update(test);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }

            return StatusCode(StatusCodes.Status202Accepted);
        }

        [HttpPost]
        public HttpResponseMessage ArchiveTest(long id)
        {
            try
            {
                var test = _testRepository.FindById(id);
                test.IsArchived = true;
                _testRepository.Update(test);
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }

            return new HttpResponseMessage(HttpStatusCode.Accepted);
        }
    }
}