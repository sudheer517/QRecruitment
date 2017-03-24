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
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using AspNetCoreSpa.Server.Services.Abstract;
using AspNetCoreSpa;

namespace Quantium.Recruitment.ApiServices.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("[controller]/[action]/{id?}")]
    public class TestController : Controller
    {
        private readonly IEntityBaseRepository<Job> _jobRepository;
        private readonly IEntityBaseRepository<Candidate> _candidateRepository;
        private readonly IEntityBaseRepository<Candidate_Job> _candidateJobRepository;
        private readonly IEntityBaseRepository<Test> _testRepository;
        private readonly IEntityBaseRepository<Job_Difficulty_Label> _jobDifficultyLabelRepository;
        private readonly IEntityBaseRepository<Question> _questionRepository;
        private readonly IEntityBaseRepository<Challenge> _challengeRepository;
        private readonly IEntityBaseRepository<Admin> _adminRepository;
        private readonly IEmailSender _emailSender;


        public TestController(
            IEntityBaseRepository<Job> jobRepository, 
            IEntityBaseRepository<Candidate> candidateRepository,
            IEntityBaseRepository<Candidate_Job> candidateJobRepository,
            IEntityBaseRepository<Test> testRepository,
            IEntityBaseRepository<Job_Difficulty_Label> jobDifficultyLabelRepository,
            IEntityBaseRepository<Question> questionRepository,
            IEntityBaseRepository<Challenge> challengeRepository,
            IEntityBaseRepository<Admin> adminRepository,
            IEmailSender emailSender)
        {
            _jobRepository = jobRepository;
            _candidateRepository = candidateRepository;
            _candidateJobRepository = candidateJobRepository;
            _testRepository = testRepository;
            _jobDifficultyLabelRepository = jobDifficultyLabelRepository;
            _questionRepository = questionRepository;
            _challengeRepository = challengeRepository;
            _emailSender = emailSender;
            _adminRepository = adminRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Generate([FromBody]List<Candidate_JobDto> candidatesJobsDto)
        {
            try
            {
                IList<string> emails = new List<string>();

                var candidatesJobs = Mapper.Map<List<Candidate_Job>>(candidatesJobsDto);
                var job = _jobRepository.GetSingle(candidatesJobsDto.First().Job.Id);

                foreach (var candidateJobDto in candidatesJobsDto)
                {
                    var candidate = _candidateRepository.GetSingle(c => c.Id == candidateJobDto.Candidate.Id, cj => cj.CandidateJobs, cj => cj.Tests);

                    var newCandidateJob = new Candidate_Job
                    {
                        Job = job,
                        Candidate = candidate
                    };

                    var candidateJob = candidate.CandidateJobs.SingleOrDefault(cj => cj.CandidateId == candidate.Id && cj.JobId == job.Id);

                    if (candidateJob == null)
                        _candidateJobRepository.Add(newCandidateJob);

                    Test newTest = new Test
                    {
                        Name = job.Title + candidate.FirstName,
                        Candidate = candidate,
                        Job = job,
                        CreatedUtc = DateTime.UtcNow,
                        CreatedByUserId = _adminRepository.GetSingle(a => a.Email == this.User.Identities.First().Name).Id
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

                    IList<Job_Difficulty_Label> jobDifficultyLabels = _jobDifficultyLabelRepository.FindBy(id => id.JobId == job.Id).ToList();

                    List<Question> selectedQuestions = new List<Question>();

                    foreach (var jobDiffLabel in jobDifficultyLabels)
                    {
                        var questions =
                            _questionRepository
                            .AllIncluding(q => q.Difficulty, q => q.Label)
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

                    emails.Add(candidate.Email);
                }
                _challengeRepository.CommitAsync();

                await SendEmails(emails);

                return Created(string.Empty, JsonConvert.SerializeObject("Tests created"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }           
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var allTests = _testRepository.AllIncluding(t => t.Candidate, t => t.Job).Where(t => t.IsArchived != true).OrderByDescending(t => t.FinishedDate).ToList();
            var allTestDtos = Mapper.Map<List<TestDto>>(allTests);

            return Ok(allTestDtos);
        }

        [HttpGet]
        public IActionResult GetTestResults()
        {
            var allTests = 
                _testRepository.
                FindByIncludeAll(t => t.IsArchived != true).
                OrderByDescending(t => t.FinishedDate).
                ToList();

            IList<TestResultDto> allTestResultDtos = new List<TestResultDto>();

            foreach (var test in allTests)
            {
                var testDto = Mapper.Map<TestDto>(test);

                if (test.IsFinished)
                {
                    FillTestDto(test, testDto);
                }

                TestResultDto testResult = new TestResultDto
                {
                    Id = testDto.Id,
                    Candidate = testDto.Candidate.FirstName + " " + testDto.Candidate.LastName,
                    Email = testDto.Candidate.Email,
                    JobApplied = testDto.Job.Title,
                    FinishedDate = testDto.FinishedDate,
                    Result = testDto.IsFinished ? testDto.IsTestPassed ? "Passed" : "Failed" : string.Empty ,
                    College = testDto.Candidate.College,
                    CGPA = testDto.Candidate.CGPA,
                    TotalRightAnswers = testDto.TotalRightAnswers,
                    IsFinished = testDto.IsFinished
                };

                allTestResultDtos.Add(testResult);
            }

            return Ok(allTestResultDtos);

        }

        [HttpPost]
        public IActionResult GetFinishedTestDetail([FromBody]long testId)
        {
            var finishedTest =
                _testRepository.GetSingle(t => t.IsArchived != true && t.Id == testId);

            var finishedTestDto = Mapper.Map<TestDto>(finishedTest);
            FillTestDto(finishedTest, finishedTestDto);
            return Ok(finishedTestDto);
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

        private bool UpdateCandidatesForTest(List<Candidate_JobDto> candidateJob)
        {
            try
            {
                var candidateList = candidateJob.Select(x => x.Candidate.Id).ToList();
                var candidates = _candidateRepository.GetAll().Where(c => candidateList.Contains(c.Id));
                foreach (var candidate in candidates.ToList())
                {
                    //TestMailSent Stages
                    //Default =0
                    //TestCreated=2
                    //TestMailSent=3
                    if (!(candidate.TestMailSent == 2))
                    {
                        candidate.TestMailSent = 2;
                        var updatedCandidate = (Candidate)Mapper.Map(candidate, candidate, typeof(Candidate), typeof(Candidate));
                        _candidateRepository.Update(updatedCandidate);
                        _candidateRepository.Commit();
                    }
                }
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }

        private async Task<bool> SendEmails(IList<string> emails)
        {
            var emailTemplate = System.IO.File.ReadAllText(@"Server/Templates/TestCreationEmailTemplate.html");

            var socialLogins = new List<string>()
            {
                "@outlook", "@live", "@hotmail", "@gmail", "@google"
            };

            foreach (var email in emails)
            {
                string loginProviderMessage = string.Empty;

                if (!socialLogins.Any(emailType => email.Contains(emailType)))
                {
                    loginProviderMessage = "the credentials sent to you";
                }
                else
                {
                    loginProviderMessage = "your social login(this should match the email you registered with us)";
                }
                var emailTask = _emailSender.SendEmailAsync(new EmailModel
                {
                    To = email,
                    From = Startup.Configuration["RecruitmentAdminEmail"],
                    DisplayName = "Quantium Recruitment",
                    Subject = "Test generated for you",
                    HtmlBody = string.Format(emailTemplate, loginProviderMessage)
                });

                await Task.Run(() => emailTask);
            }

            return true;
        }
    }
}