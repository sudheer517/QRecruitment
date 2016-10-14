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

        public JobController(IJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
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

            _jobRepository.Add(job);

            return Ok(Mapper.Map<JobDto>(job));
        }
    }
}