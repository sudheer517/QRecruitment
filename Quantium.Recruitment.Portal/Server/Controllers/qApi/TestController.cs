﻿using Quantium.Recruitment.Infrastructure;
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


        public TestController(
            IEntityBaseRepository<Job> jobRepository, 
            IEntityBaseRepository<Candidate> candidateRepository,
            IEntityBaseRepository<Candidate_Job> candidateJobRepository,
            IEntityBaseRepository<Test> testRepository,
            IEntityBaseRepository<Job_Difficulty_Label> jobDifficultyLabelRepository,
            IEntityBaseRepository<Question> questionRepository,
            IEntityBaseRepository<Challenge> challengeRepository)
        {
            _jobRepository = jobRepository;
            _candidateRepository = candidateRepository;
            _candidateJobRepository = candidateJobRepository;
            _testRepository = testRepository;
            _jobDifficultyLabelRepository = jobDifficultyLabelRepository;
            _questionRepository = questionRepository;
            _challengeRepository = challengeRepository;

        }

        [HttpPost]
        public IActionResult Generate([FromBody]List<Candidate_JobDto> candidatesJobsDto)
        {
            try
            {
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

                }
                _challengeRepository.CommitAsync();
                var candidateReponse = UpdateCandidatesForTest(candidatesJobsDto);
                if (candidateReponse)
                    return Created(string.Empty, JsonConvert.SerializeObject("test created"));
                else
                    throw new Exception("Test creation failed");
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
    }
}