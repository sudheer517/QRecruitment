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
    public class ChallengeController : ApiController
    {
        private readonly IChallengeRepository _challengeRepository;
        private readonly ITestRepository _testRepository;
        private readonly ICandidateSelectedOptionRepository _candidateSelectedOptionRepository;

        public ChallengeController(
            IChallengeRepository challengeRepository,
            ITestRepository testRepository,
            ICandidateSelectedOptionRepository candidateSelectedOptionRepository)
        {
            _challengeRepository = challengeRepository;
            _testRepository = testRepository;
            _candidateSelectedOptionRepository = candidateSelectedOptionRepository;
        }

        [HttpGet]
        public IHttpActionResult GetAllChallenges()
        {
            var challenges = _challengeRepository.GetAll().ToList();

            var cDtos = Mapper.Map<List<ChallengeDto>>(challenges);

            return Ok(cDtos);
        }

        [HttpGet]
        public IHttpActionResult GetNext([FromUri]string email)
        {
            var test = _testRepository.FindByCandidateEmail(email);
            var challenges = test.Challenges.Where(c => c.TestId == test.Id && c.IsSent != true).OrderBy(c => c.Id).ToList();
            var challenge = challenges.Count() > 0 ? challenges.First() : null;
            

            if(challenge == null)
            {
                return Ok("Finished");
            }

            challenge.Question.Options.ForEach(o => o.IsAnswer = false);

            return Ok(Mapper.Map<ChallengeDto>(challenge));
        }

        [HttpPost]
        public IHttpActionResult PostChallenge([FromBody]ChallengeDto challengeDto)
        {
            //var challenge = Mapper.Map<Challenge>(challengeDto);

            var challenge = _challengeRepository.FindById(challengeDto.Id);
            challenge.IsSent = true;
            challenge.IsAnswered = challengeDto.CandidateSelectedOptions.Count > 0 ? true : false;

            challenge.CandidateSelectedOptions = Mapper.Map<List<CandidateSelectedOption>>(challengeDto.CandidateSelectedOptions);
            //challenge.CandidateSelectedOptions.ForEach(item => item.Challenge = challenge);

            //challenge.Question.Challenges = new List<Challenge>();
            //challenge.Question.Challenges.Add(challenge);

            _challengeRepository.Update(challenge);
            return Ok("success");
        }
    }
}