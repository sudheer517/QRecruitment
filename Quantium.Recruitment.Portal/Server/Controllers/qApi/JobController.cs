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

namespace Quantium.Recruitment.ApiServices.Controllers
{
    [Route("[controller]/[action]/{id?}")]
    public class JobController : Controller
    {
        private readonly IEntityBaseRepository<Job> _jobRepository;
        private readonly IEntityBaseRepository<Department> _departmentRepository;
        private readonly IEntityBaseRepository<Label> _labelRepository;
        private readonly IEntityBaseRepository<Difficulty> _difficultyRepository;

        public JobController(
            IEntityBaseRepository<Job> jobRepository, 
            IEntityBaseRepository<Department> departmentRepository,
            IEntityBaseRepository<Label> labelRepository,
            IEntityBaseRepository<Difficulty> difficultyRepository)
        {
            _jobRepository = jobRepository;
            _departmentRepository = departmentRepository;
            _labelRepository = labelRepository;
            _difficultyRepository = difficultyRepository;
        }

        [HttpPost]
        public IActionResult Create([FromBody]JobDto jobDto)
        {
            try
            {
                var job = Mapper.Map<Job>(jobDto);

                var department = _departmentRepository.GetSingle(job.Department.Id);

                foreach (var jobDifficultyLabel in job.JobDifficultyLabels)
                {
                    var label = _labelRepository.GetSingle(jobDifficultyLabel.Label.Id);
                    var difficulty = _difficultyRepository.GetSingle(jobDifficultyLabel.Difficulty.Id);
                    jobDifficultyLabel.Label = label;
                    jobDifficultyLabel.Difficulty = difficulty;
                }

                job.CreatedUtc = DateTime.UtcNow;
                job.Department = department;
                _jobRepository.Add(job);
                var responseDto = Mapper.Map<JobDto>(job);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
            return Created(string.Empty, jobDto);
        }

        [HttpPost]
        public IActionResult Delete([FromBody]JobDto jobDto)
        {
            try
            {
                var job = Mapper.Map<Job>(jobDto);
                _jobRepository.Delete(job);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
            return Ok(JsonConvert.SerializeObject("deleted"));
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var jobs = _jobRepository.GetAll().OrderByDescending(j => j.CreatedUtc).ToList();

            var jDtos = Mapper.Map<List<JobDto>>(jobs);

            return Ok(jDtos);
        }
    }
}