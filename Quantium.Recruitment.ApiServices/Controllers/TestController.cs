using Quantium.Recruitment.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using Quantium.Recruitment.ApiServices.Models;
using Quantium.Recruitment.Entities;
using Quantium.Recruitment.Infrastructure.Repositories;
namespace Quantium.Recruitment.ApiServices.Controllers
{
    public class TestController : ApiController
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
        public IHttpActionResult GenerateTests(List<Candidate_JobDto> candidatesJobsDto)
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
                
                if(candidateJob == null)
                    _candidateJobRepository.Add(newCandidateJob);

                Test newTest = new Test
                {
                    Name = job.Title + candidate.FirstName,
                    Candidate = candidate,
                    Job = job
                };

                var activeTest = candidate.Tests.FirstOrDefault(t => t.IsFinished != true && t.Job.Id == job.Id);

                if (activeTest == null)
                {
                    _testRepository.Add(newTest);
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

                    if(availableQuestionCount < jobDiffLabel.QuestionCount)
                    {
                        throw new Exception("Question count exceeds available questions count");
                    }

                    var randomQuestions = questions.OrderBy(item => Guid.NewGuid()).Take(jobDiffLabel.QuestionCount);

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

            return Ok();
        }

        [HttpGet]
        public IHttpActionResult HasActiveTestForCandidate([FromUri]string email)
        {
            var activeTest = _testRepository.FindActiveTestByCandidateEmail(email);

            if (activeTest == null)
                return Ok(false);
            else
                return Ok(true);
        }
    }
}