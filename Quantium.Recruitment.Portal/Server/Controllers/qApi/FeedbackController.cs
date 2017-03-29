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
    public class FeedbackController : Controller
    {
        private readonly IEntityBaseRepository<Feedback> _feedbackRepository;
        private readonly IEntityBaseRepository<Test> _testRepository;
        private readonly IEntityBaseRepository<Candidate> _candidateRepository;

        public FeedbackController(
            IEntityBaseRepository<Feedback> feedbackRepository,
            IEntityBaseRepository<Test> testRepository,
            IEntityBaseRepository<Candidate> candidateRepository)
        {
            _feedbackRepository = feedbackRepository;
            _testRepository = testRepository;
            _candidateRepository = candidateRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var feedbacks = await _feedbackRepository.GetAllAsync();

            var fDtos = Mapper.Map<List<FeedbackDto>>(feedbacks.ToList());

            return Ok(fDtos);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]FeedbackDto feedbackDto)
        {
            var feedback = Mapper.Map<Feedback>(feedbackDto);
            var candidateEmail = this.User.Identities.First().Name;

            var candidate = await _candidateRepository.GetSingleAsync(c => c.Email == candidateEmail);


            if (feedback.TestId == 0)
            {
                var test = await _testRepository.GetSingleAsync(t => t.Candidate.Email == candidateEmail);

                feedback.TestId = test != null ? test.Id : 0;
            }

            feedback.CandidateId = candidate.Id;
            _feedbackRepository.Add(feedback);
            _feedbackRepository.Commit();

            return Created(string.Empty, feedback);
        }
    }
}