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
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace Quantium.Recruitment.ApiServices.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("[controller]/[action]/{id?}")]
    public class JobController : Controller
    {
        private readonly IEntityBaseRepository<Job> _jobRepository;
        private readonly IEntityBaseRepository<Department> _departmentRepository;
        private readonly IEntityBaseRepository<Label> _labelRepository;
        private readonly IEntityBaseRepository<Difficulty> _difficultyRepository;
        private readonly IEntityBaseRepository<Admin> _adminRepository;

        public JobController(
            IEntityBaseRepository<Job> jobRepository, 
            IEntityBaseRepository<Department> departmentRepository,
            IEntityBaseRepository<Label> labelRepository,
            IEntityBaseRepository<Difficulty> difficultyRepository,
            IEntityBaseRepository<Admin> adminRepository)
        {
            _jobRepository = jobRepository;
            _departmentRepository = departmentRepository;
            _labelRepository = labelRepository;
            _difficultyRepository = difficultyRepository;
            _adminRepository = adminRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]JobDto jobDto)
        {
            try
            {
                var job = Mapper.Map<Job>(jobDto);
                var department = await _departmentRepository.GetSingleAsync(j => j.Id == job.Department.Id);

                foreach (var jobDifficultyLabel in job.JobDifficultyLabels)
                {
                    var label = await _labelRepository.GetSingleAsync(l => l.Id == jobDifficultyLabel.Label.Id);
                    var difficulty = await _difficultyRepository.GetSingleAsync(d => d.Id == jobDifficultyLabel.Difficulty.Id);
                    jobDifficultyLabel.Label = label;
                    jobDifficultyLabel.Difficulty = difficulty;
                }

                var adminEmail = this.User.Identities.First().Name;
                var admin = await _adminRepository.GetSingleAsync(a => a.Email == adminEmail);

                job.CreatedUtc = DateTime.UtcNow;
                job.IsActive = true;
                job.Department = department;
                job.CreatedByUserId = admin.Id;
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
        public async Task<IActionResult> Delete([FromBody]long jobId)
        {
            try
            {
                var job = await _jobRepository.GetSingleAsync(j => j.Id == jobId);
                job.IsActive = false;
                _jobRepository.Edit(job);
                _jobRepository.Commit();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
            return Ok(JsonConvert.SerializeObject("deleted"));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var jobs = await _jobRepository.FindByAsync(j => j.IsActive != false);

            jobs = jobs.OrderByDescending(j => j.CreatedUtc).ToList();

            var jDtos = Mapper.Map<List<JobDto>>(jobs);

            return Ok(jDtos);
        }

        [HttpGet]
        public async Task<IActionResult> IsJobExists([FromQuery]string title)
        {
            var job = await _jobRepository.GetSingleAsync(j => j.Title == title);

            if (job != null)
                return Ok(JsonConvert.SerializeObject(true));
            else
                return Ok(JsonConvert.SerializeObject(false));

        }
    }
}