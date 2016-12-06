using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using AutoMapper;
using Quantium.Recruitment.ApiServices.Models;
using Quantium.Recruitment.Entities;
using Quantium.Recruitment.Infrastructure.Repositories;
using Quantium.Recruitment.Infrastructure.Unity;
using Microsoft.AspNetCore.Mvc;

namespace Quantium.Recruitment.ApiServices.Controllers
{
    //[Authorize]
    [Route("api/job")]
    public class JobController : Controller
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
        public IActionResult GetAllJobs()
        {
            var jobs = _jobRepository.GetAll().ToList();

            var jDtos = Mapper.Map<List<JobDto>>(jobs);
            
            return Ok(jDtos);
        }

        [HttpGet]
        public IActionResult GetSingle(int key)
        {
            var job = _jobRepository.GetAll().SingleOrDefault(item => item.Id == key);

            return Ok(Mapper.Map<JobDto>(job));
        }

        [HttpPost]
        public HttpResponseMessage Create(JobDto jobDto)
        {
            try
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

                job.CreatedUtc = DateTime.UtcNow;
                job.Department = department;
                _jobRepository.Add(job);
                var responseDto = Mapper.Map<JobDto>(job);
            }
            catch(Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
            return new HttpResponseMessage(HttpStatusCode.Created);
        }
    }
}