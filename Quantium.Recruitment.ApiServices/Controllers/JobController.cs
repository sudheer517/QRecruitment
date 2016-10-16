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
using Quantium.Recruitment.ApiServices.Helpers;
using System.Web.OData;
using System.Web.OData.Routing;
using Quantium.Recruitment.Infrastructure.Unity;
using Microsoft.Practices.Unity;

namespace Quantium.Recruitment.ApiServices.Controllers
{
    //[Authorize]
    public class JobController : ApiController
    {
        private readonly IJobRepository _jobRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IDifficultyRepository _difficultyRepository;
        private readonly ILabelRepository _labelRepostory;

        public JobController(
            IJobRepository jobRepository, 
            IDepartmentRepository departmentRepository,
            IDifficultyRepository difficultyRepository,
            ILabelRepository labelRepostory)
        {
            _jobRepository = jobRepository;
            _departmentRepository = departmentRepository;
            _difficultyRepository = difficultyRepository;
            _labelRepostory = labelRepostory;
        }

        [HttpGet]
        public IHttpActionResult GetAllJobs()
        {
            var jobs = _jobRepository.GetAll().ToList();

            var jDtos = Mapper.Map<List<JobDto>>(jobs);
            
            return Ok(jDtos);
        }

        [HttpGet]
        public IHttpActionResult GetSingle(int key)
        {
            var job = _jobRepository.GetAll().SingleOrDefault(item => item.Id == key);

            return Ok(Mapper.Map<JobDto>(job));
        }

        [HttpPost]
        public IHttpActionResult Create(JobDto jobDto)
        {
            
            var job = Mapper.Map<Job>(jobDto);

            var department = _departmentRepository.FindById(job.Department.Id);

            foreach (var jobDifficultyLabel in job.JobDifficultyLabels)
            {
                var label = _labelRepostory.FindById(jobDifficultyLabel.Label.Id);
                var difficulty = _difficultyRepository.FindById(jobDifficultyLabel.Difficulty.Id);
                jobDifficultyLabel.Label = label;
                jobDifficultyLabel.Difficulty = difficulty;
            }

            job.Department = department;
            _jobRepository.Add(job);
            return Ok(Mapper.Map<JobDto>(job));
        }
    }
}