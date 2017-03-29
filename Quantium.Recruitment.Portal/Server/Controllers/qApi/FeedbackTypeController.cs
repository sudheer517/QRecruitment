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
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace Quantium.Recruitment.ApiServices.Controllers
{
    [Authorize(Roles = "Admin, Candidate")]
    [Route("[controller]/[action]/{id?}")]
    public class FeedbackTypeController : Controller
    {
        private readonly IEntityBaseRepository<FeedbackType> _feedbackTypeRepository;

        public FeedbackTypeController(IEntityBaseRepository<FeedbackType> feedbackTypeRepository)
        {
            _feedbackTypeRepository = feedbackTypeRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var feedbacks = await _feedbackTypeRepository.GetAllAsync();

            var fDtos = Mapper.Map<List<FeedbackTypeDto>>(feedbacks.ToList());

            return Ok(fDtos);
        }
    }
}