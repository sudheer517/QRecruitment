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
using Microsoft.AspNetCore.Hosting;
using OfficeOpenXml;
using System.IO;

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
        private readonly IHostingEnvironment _env;

        public TestController(
            IEntityBaseRepository<Job> jobRepository, 
            IEntityBaseRepository<Candidate> candidateRepository,
            IEntityBaseRepository<Candidate_Job> candidateJobRepository,
            IEntityBaseRepository<Test> testRepository,
            IEntityBaseRepository<Job_Difficulty_Label> jobDifficultyLabelRepository,
            IEntityBaseRepository<Question> questionRepository,
            IEntityBaseRepository<Challenge> challengeRepository,
            IEntityBaseRepository<Admin> adminRepository,
            IEmailSender emailSender,
            IHostingEnvironment env)
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
            _env = env;
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
                            .Where(ques => ques.DifficultyId == jobDiffLabel.Difficulty.Id && ques.Label.Id == jobDiffLabel.Label.Id && ques.IsActive == true)
                            .ToList();

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
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
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
        public async Task<IActionResult> GetTestResults()
        {
            var allTests =
                await _testRepository.
                FindByIncludeAllAsync(t => t.IsArchived != true);

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
                    Result = testDto.IsFinished ? testDto.IsTestPassed ? "Passed" : "Failed" : string.Empty,
                    College = testDto.Candidate.College,
                    CGPA = testDto.Candidate.CGPA,
                    TotalRightAnswers = testDto.TotalRightAnswers,
                    IsFinished = testDto.IsFinished
                };

                allTestResultDtos.Add(testResult);
            }

            return Ok(allTestResultDtos);

        }

        [HttpGet]
        public async Task<IActionResult> GetArchivedTestResults()
        {
            var allTests =
                await _testRepository.
                FindByIncludeAllAsync(t => t.IsArchived == true);

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
                    Result = testDto.IsFinished ? testDto.IsTestPassed ? "Passed" : "Failed" : string.Empty,
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
                _testRepository.GetSingle(t => t.Id == testId);

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
                DisplayQuestionCount = jdl.DisplayQuestionCount,
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
            finishedTestDto.LabelDiffAnswers = new Dictionary<string, string>();
            foreach (var item in twoKeyJobDiffLabelMap)
            {
                if (finishedTestDto.IsTestPassed && item.PassingQuestionCount > item.AnsweredCount)
                {
                    finishedTestDto.IsTestPassed = false;
                }

                finishedTestDto.LabelDiffAnswers.Add($"{item.Label.Name}-{item.Difficulty.Name}", $"{item.AnsweredCount} out of {item.DisplayQuestionCount}");
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
            var emailTemplate = System.Net.WebUtility.HtmlDecode(System.IO.File.ReadAllText(System.IO.Path.Combine(_env.WebRootPath, "templates\\TestCreationEmailTemplate.html")));

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
                var Content = emailTemplate;
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("Message", loginProviderMessage);
                parameters.Add("candidateEmail", email);

                foreach (var param in parameters)
                {
                    Content = Content.Replace("<" + param.Key + ">", param.Value);
                }
                var emailTask = _emailSender.SendEmailAsync(new EmailModel
                {
                    To = email,
                    From = Startup.Configuration["RecruitmentAdminEmail"],
                    DisplayName = "Quantium Recruitment",
                    Subject = "Quantium recruitment test",
                    TextBody = Content
                });

                await Task.Run(() => emailTask);
            }

            return true;
        }

        [HttpGet]
        public async Task<IActionResult> ExportArchivedTests()
        {
            var tests =
                await _testRepository.
                FindByIncludeAllAsync(t => t.IsArchived == true);


            var excelPackage = new ExcelPackage();
            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("TestResults");

            int columnCount = 14;
            IList<string> headers = new List<string>()
            {
                "Id",
                "Candidate Name",
                "Email",
                "Title",
                "Test Completion Date",
                "Test Result",
                "College",
                "CGPA",
                "Branch",
                "PassingYear",
                "Mobile",
                "Total Questions Displayed",
                "Total Questions Answered",
                "Total Correct Answers",

            };

            if (columnCount != headers.Count)
            {
                throw new Exception("Header count not equal columns");
            }

            for (int column = 1; column <= columnCount; column++)
            {
                worksheet.SetValue(1, column, headers[column - 1]);
            }

            for (int testIndex = 0, rowIndex = 2; testIndex < tests.Count; testIndex++, rowIndex++)
            {
                var test = tests[testIndex];
                var testDto = Mapper.Map<TestDto>(test);

                if (testDto.IsFinished)
                {
                    FillTestDto(tests[testIndex], testDto);
                }

                worksheet.SetValue(rowIndex, 1, testIndex + 1);
                worksheet.SetValue(rowIndex, 2, $"{testDto.Candidate.FirstName} {testDto.Candidate.LastName}");
                worksheet.SetValue(rowIndex, 3, testDto.Candidate.Email);
                worksheet.SetValue(rowIndex, 4, testDto.Job.Title);
                if (testDto.IsFinished)
                    worksheet.SetValue(rowIndex, 5, testDto.FinishedDate.ToLocalTime().ToString("M/d/yyyy h:mm:ss tt"));
                else
                    worksheet.SetValue(rowIndex, 5, "N/A");
                worksheet.SetValue(rowIndex, 6, testDto.IsFinished ? testDto.IsTestPassed ? "Passed" : "Failed" : "Not Finished");
                worksheet.SetValue(rowIndex, 7, testDto.Candidate.College);
                worksheet.SetValue(rowIndex, 8, testDto.Candidate.CGPA);
                worksheet.SetValue(rowIndex, 9, testDto.Candidate.Branch);
                worksheet.SetValue(rowIndex, 10, testDto.Candidate.PassingYear);
                worksheet.SetValue(rowIndex, 11, testDto.Candidate.Mobile);
                worksheet.SetValue(rowIndex, 12, testDto.TotalChallengesDisplayed);
                worksheet.SetValue(rowIndex, 13, testDto.TotalChallengesAnswered);
                worksheet.SetValue(rowIndex, 14, testDto.TotalRightAnswers);

            }

            var bytesArray = excelPackage.GetAsByteArray();
            var stream = excelPackage.Stream;

            FileContentResult fileResult = new FileContentResult(bytesArray, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                FileDownloadName = "TestResults.xlsx"
            };

            return fileResult;
        }

        [HttpGet]
        public async Task<IActionResult> ExportAllTests()
        {

            var tests = 
                await _testRepository.
                FindByIncludeAllAsync(t => t.IsArchived != true);


            var excelPackage = new ExcelPackage();
            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("TestResults");

            int columnCount = 14;
            IList<string> headers = new List<string>()
            {
                "Id",
                "Candidate Name",
                "Email",
                "Title",
                "Test Completion Date",
                "Test Result",
                "College",
                "CGPA",
                "Branch",
                "PassingYear",
                "Mobile",
                "Total Questions Displayed",
                "Total Questions Answered",
                "Total Correct Answers",

            };

            if(columnCount != headers.Count)
            {
                throw new Exception("Header count not equal columns");
            }

            for (int column = 1; column <= columnCount; column++)
            {
                worksheet.SetValue(1, column, headers[column - 1]);
            }

            for (int testIndex = 0, rowIndex = 2; testIndex < tests.Count; testIndex++, rowIndex++)
            {
                var test = tests[testIndex];
                var testDto = Mapper.Map<TestDto>(test);

                if (testDto.IsFinished)
                {
                    FillTestDto(tests[testIndex], testDto);
                }

                worksheet.SetValue(rowIndex, 1, testIndex + 1);
                worksheet.SetValue(rowIndex, 2, $"{testDto.Candidate.FirstName} {testDto.Candidate.LastName}");
                worksheet.SetValue(rowIndex, 3, testDto.Candidate.Email);
                worksheet.SetValue(rowIndex, 4, testDto.Job.Title);
                if (testDto.IsFinished)
                    worksheet.SetValue(rowIndex, 5, testDto.FinishedDate.ToLocalTime().ToString("M/d/yyyy h:mm:ss tt"));
                else
                    worksheet.SetValue(rowIndex, 5, "N/A");
                worksheet.SetValue(rowIndex, 6, testDto.IsFinished ? testDto.IsTestPassed ? "Passed" : "Failed" : "Not Finished");
                worksheet.SetValue(rowIndex, 7, testDto.Candidate.College);
                worksheet.SetValue(rowIndex, 8, testDto.Candidate.CGPA);
                worksheet.SetValue(rowIndex, 9, testDto.Candidate.Branch);
                worksheet.SetValue(rowIndex, 10, testDto.Candidate.PassingYear);
                worksheet.SetValue(rowIndex, 11, testDto.Candidate.Mobile);
                worksheet.SetValue(rowIndex, 12, testDto.TotalChallengesDisplayed);
                worksheet.SetValue(rowIndex, 13, testDto.TotalChallengesAnswered);
                worksheet.SetValue(rowIndex, 14, testDto.TotalRightAnswers);

            }

            var bytesArray = excelPackage.GetAsByteArray();
            var stream = excelPackage.Stream;

            FileContentResult fileResult = new FileContentResult(bytesArray, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                FileDownloadName = "TestResults.xlsx"
            };

            return fileResult;
        }

        [HttpGet]
        public async Task<IActionResult> ExportFinishedTestsByJob([FromQuery]long jobId)
        {

            var tests =
                await _testRepository.
                FindByIncludeAllAsync(t => t.IsArchived != true && t.Job.Id == jobId && t.IsFinished);

            var job = await _jobRepository.GetSingleAsyncIncluding(j => j.Id == jobId, j => j.JobDifficultyLabels);

            var excelPackage = new ExcelPackage();
            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("TestResults");

            int columnCount = 14;
            IList<string> headers = new List<string>()
            {
                "Id",
                "Candidate Name",
                "Email",
                "Title",
                "Test Completion Date",
                "Test Result",
                "College",
                "CGPA",
                "Branch",
                "PassingYear",
                "Mobile",
                "Total Questions Displayed",
                "Total Questions Answered",
                "Total Correct Answers",

            };

            if (job.JobDifficultyLabels != null)
            {
                foreach (var jdl in job.JobDifficultyLabels)
                {
                    headers.Add($"{jdl.Label.Name} - {jdl.Difficulty.Name}");
                }
            }

            for (int column = 1; column <= headers.Count; column++)
            {
                worksheet.SetValue(1, column, headers[column - 1]);
            }

            for (int testIndex = 0, rowIndex = 2; testIndex < tests.Count; testIndex++, rowIndex++)
            {
                var test = tests[testIndex];
                var testDto = Mapper.Map<TestDto>(test);

                if (testDto.IsFinished)
                {
                    FillTestDto(tests[testIndex], testDto);
                }

                worksheet.SetValue(rowIndex, 1, testIndex + 1);
                worksheet.SetValue(rowIndex, 2, $"{testDto.Candidate.FirstName} {testDto.Candidate.LastName}");
                worksheet.SetValue(rowIndex, 3, testDto.Candidate.Email);
                worksheet.SetValue(rowIndex, 4, testDto.Job.Title);
                if (testDto.IsFinished)
                    worksheet.SetValue(rowIndex, 5, testDto.FinishedDate.ToLocalTime().ToString("M/d/yyyy h:mm:ss tt"));
                else
                    worksheet.SetValue(rowIndex, 5, "N/A");
                worksheet.SetValue(rowIndex, 6, testDto.IsFinished ? testDto.IsTestPassed ? "Passed" : "Failed" : "Not Finished");
                worksheet.SetValue(rowIndex, 7, testDto.Candidate.College);
                worksheet.SetValue(rowIndex, 8, testDto.Candidate.CGPA);
                worksheet.SetValue(rowIndex, 9, testDto.Candidate.Branch);
                worksheet.SetValue(rowIndex, 10, testDto.Candidate.PassingYear);
                worksheet.SetValue(rowIndex, 11, testDto.Candidate.Mobile);
                worksheet.SetValue(rowIndex, 12, testDto.TotalChallengesDisplayed);
                worksheet.SetValue(rowIndex, 13, testDto.TotalChallengesAnswered);
                worksheet.SetValue(rowIndex, 14, testDto.TotalRightAnswers);

                if (job.JobDifficultyLabels != null)
                {
                    FillCutOffByLabelAndDiff(test, testDto, worksheet, rowIndex, 15);
                }
            }

            var bytesArray = excelPackage.GetAsByteArray();
            var stream = excelPackage.Stream;

            FileContentResult fileResult = new FileContentResult(bytesArray, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                FileDownloadName = "TestResults.xlsx"
            };

            return fileResult;
        }

        private void FillCutOffByLabelAndDiff(Test test, TestDto testDto, ExcelWorksheet worksheet, int rowIndex, int startAtColumn)
        {

            var jobDiffLabels = Mapper.Map<List<Job_Difficulty_LabelDto>>(test.Job.JobDifficultyLabels);

            var twoKeyJobDiffLabelMap = jobDiffLabels.Select(jdl => new Job_Difficulty_LabelDto
            {
                Label = jdl.Label,
                Difficulty = jdl.Difficulty,
                PassingQuestionCount = jdl.PassingQuestionCount,
                AnsweredCount = 0
            }).ToList();

            var answeredChallenges = test.Challenges.Where(c => c.IsAnswered == true);

            foreach (var answeredChallenge in answeredChallenges)
            {
                var answersIds = answeredChallenge.Question.Options.Where(o => o.IsAnswer == true).Select(o => o.Id);
                var candidateAnswersIds = answeredChallenge.CandidateSelectedOptions.Select(cso => cso.OptionId);

                if (answersIds.Intersect(candidateAnswersIds).Count() == answersIds.Count() && answersIds.Count() == candidateAnswersIds.Count())
                {
                    var jobDiffLabel =
                        twoKeyJobDiffLabelMap.Single(
                          item =>
                              item.Difficulty.Id == answeredChallenge.Question.DifficultyId &&
                              item.Label.Id == answeredChallenge.Question.LabelId);

                    string labelName = jobDiffLabel.Label.Name;

                    jobDiffLabel.AnsweredCount += 1;
                }
            }

            for (int itemIndex = 0 ; itemIndex < twoKeyJobDiffLabelMap.Count; itemIndex++)
            {
                var item = twoKeyJobDiffLabelMap[itemIndex];

                worksheet.SetValue(rowIndex, startAtColumn + itemIndex, item.AnsweredCount);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ArchiveTests([FromBody]long[] testIds)
        {
            var tests = await _testRepository.FindByAsync(t => testIds.Contains(t.Id));

            try
            {
                foreach (var test in tests)
                {
                    test.IsArchived = true;
                }

                _testRepository.Commit();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

            return Ok(JsonConvert.SerializeObject("Deleted"));
        }
    }
}