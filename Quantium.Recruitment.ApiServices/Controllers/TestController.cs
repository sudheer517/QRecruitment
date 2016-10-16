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
using System.Web.OData;
using System.Web.OData.Routing;
namespace Quantium.Recruitment.ApiServices.Controllers
{
    public class TestController : ApiController
    {
        private readonly ITestRepository _testRepository;
        private readonly IJobRepository _jobRepository;
        private readonly ICandidateRepository _candidateRepository;
        private readonly ICandidateJobRepository _candidateJobRepository;
        public TestController(
            ITestRepository testRepository, 
            IJobRepository jobRepository,
            ICandidateRepository candidateRepository,
            ICandidateJobRepository candidateJobRepository)
        {
            _testRepository = testRepository;
            _jobRepository = jobRepository;
            _candidateRepository = candidateRepository;
            _candidateJobRepository = candidateJobRepository;
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

                _candidateJobRepository.Add(newCandidateJob);
            }

            return Ok();
        }
    }
}