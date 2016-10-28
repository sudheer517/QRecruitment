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
using System.Collections;

namespace Quantium.Recruitment.ApiServices.Controllers
{
    public class ChallengeController : ApiController
    {
        private readonly IChallengeRepository _challengeRepository;
        private readonly ITestRepository _testRepository;
        private readonly ICandidateRepository _candidateRepository;
        private readonly IOptionRepository _optionRepository;
        private readonly ICandidateSelectedOptionRepository _candidateSelectedOptionRepository;

        public ChallengeController(
            IChallengeRepository challengeRepository,
            ITestRepository testRepository,
            ICandidateSelectedOptionRepository candidateSelectedOptionRepository,
            ICandidateRepository candidateRepository,
            IOptionRepository optionRepository)
        {
            _challengeRepository = challengeRepository;
            _testRepository = testRepository;
            _candidateSelectedOptionRepository = candidateSelectedOptionRepository;
            _candidateRepository = candidateRepository;
            _optionRepository = optionRepository;
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

            if(test.Challenges == null)
            {
                test.IsFinished = true;
                _testRepository.Update(test);
                return Ok("Finished");
            }

            var challenges = test.Challenges.Where(c => c.TestId == test.Id).OrderBy(c => c.Id).ToList();

            var totalCount = challenges.Count;
            var notSentChallenges = challenges.Where(c => c.IsSent != true);

            bool[] totalChallengesAnswered = new bool[challenges.Count];

            for (int i = 0; i < challenges.Count; i++)
            {
                totalChallengesAnswered[i] = (challenges[i].IsAnswered == null || challenges[i].IsAnswered == false) ? false: true;
            }

            var currentChallenge = notSentChallenges.Count() > 0 ? notSentChallenges.OrderBy(c => c.Id).First() : null;

            if(currentChallenge == null)
            {
                test.IsFinished = true;
                _testRepository.Update(test);
                return Ok("Finished");
            }

            currentChallenge.Question.Options.ForEach(o => o.IsAnswer = false);
            var currentChallengeDto = Mapper.Map<ChallengeDto>(currentChallenge);

            currentChallengeDto.RemainingChallenges = notSentChallenges.Count() - 1;
            currentChallengeDto.currentChallenge = totalCount - notSentChallenges.Count() + 1;
            currentChallengeDto.ChallengesAnswered = totalChallengesAnswered;
            if(currentChallengeDto.RemainingChallenges == 0)
            {
                test.IsFinished = true;
                _testRepository.Update(test);
            }

            return Ok(currentChallengeDto);
        }

        [HttpPost]
        public IHttpActionResult PostChallenge([FromBody]ChallengeDto challengeDto)
        {
            //var challenge = Mapper.Map<Challenge>(challengeDto);

            var challenge = _challengeRepository.FindById(challengeDto.Id);
            challenge.IsSent = true;
            challenge.IsAnswered = challengeDto.CandidateSelectedOptions.Count > 0 ? true : false;
            challenge.StartTime = challengeDto.StartTime;
            challenge.AnsweredTime = challengeDto.AnsweredTime;
            challenge.CandidateSelectedOptions = Mapper.Map<List<CandidateSelectedOption>>(challengeDto.CandidateSelectedOptions);
            //challenge.CandidateSelectedOptions.ForEach(item => item.Challenge = challenge);

            foreach (var item in challenge.CandidateSelectedOptions)
            {
                item.Challenge = challenge;
                item.Option = _optionRepository.FindById(item.OptionId);
            }
            //challenge.Question.Challenges = new List<Challenge>();
            //challenge.Question.Challenges.Add(challenge);

            _challengeRepository.Update(challenge);
            return Ok("success");
        }
    }
}