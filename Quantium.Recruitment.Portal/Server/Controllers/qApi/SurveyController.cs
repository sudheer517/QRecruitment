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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace Quantium.Recruitment.ApiServices.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("[controller]/[action]/{id?}")]
    public class SurveyController : Controller
     {
        private readonly IEntityBaseRepository<Survey> _surveyRepository;
        private readonly IEntityBaseRepository<Candidate> _candidateRepository;
        private readonly IEntityBaseRepository<SurveyChallenge> _surveyChallengeRepository;
        private readonly IEntityBaseRepository<SurveyQuestion> _surveyQuestionRepository;
 
         public SurveyController(
            IEntityBaseRepository<Candidate> candidateRepository,
            IEntityBaseRepository<Survey> surveyRepository,
            IEntityBaseRepository<SurveyChallenge> surveyChallengeRepository,
             IEntityBaseRepository<SurveyQuestion> surveyQuestionRepository)
         {
            _candidateRepository = candidateRepository;
            _surveyRepository = surveyRepository;
            _surveyChallengeRepository = surveyChallengeRepository;
            _surveyQuestionRepository = surveyQuestionRepository;
         }
 
         [HttpPost]
        public IActionResult GenerateSurveys([FromBody]List<Candidate_SurveyDto> candidateSurveysDto)
        {
            try
            {
                var candidatesSurveys = Mapper.Map<List<Candidate_Survey>>(candidateSurveysDto);
               
                foreach (var candidateSurveyDto in candidateSurveysDto)
                {
                    var candidate = _candidateRepository.GetSingle(candidateSurveyDto.Candidate.Id);

                    Survey newSurvey = new Survey
                    {
                        Candidate = candidate,
                        CreatedUtc = DateTime.UtcNow
                    };

                    var activeSurvey = candidate.Surveys.FirstOrDefault(s => s.IsFinished != true);

                    if (activeSurvey == null)
                    {
                        _surveyRepository.Add(newSurvey);
                    }
                    else
                    {
                        continue;
                    }

                    IList<SurveyQuestion> surveyQuestions = new List<SurveyQuestion>();
                    surveyQuestions = _surveyQuestionRepository.GetAll().ToList();
                    var availableQuestionCount = surveyQuestions.Count();
                    foreach (var surveyQuestion in surveyQuestions)
                    {
                        SurveyChallenge newChallenge = new SurveyChallenge
                        {
                            Survey = activeSurvey == null ? newSurvey : activeSurvey,
                            SurveyQuestion = surveyQuestion
                        };
                        _surveyChallengeRepository.Add(newChallenge);

                    }
               }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

             return StatusCode(StatusCodes.Status201Created);
         }

        [HttpPost]
        public IActionResult ArchiveSurveys([FromBody] long[] surveyIds)
        {
            try
            {
                foreach (var surveyId in surveyIds)
                {
                    var survey = _surveyRepository.GetSingle(surveyId);
                    survey.IsArchived = true;
                    _surveyRepository.Edit(survey);
               }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok();
        }

        [HttpGet]
        public IActionResult GetFinishedSurveys()
        {
            var finishedSurveys = _surveyRepository.GetAll().Where(s => s.IsFinished == true && s.IsArchived != true).OrderByDescending(t => t.FinishedDate).ToList();
            var finishedSurveyDtos = Mapper.Map<List<SurveyDto>>(finishedSurveys);
            finishedSurveyDtos.ForEach(finishedSurveyDto =>
            {
                var finishedSurvey = finishedSurveys.SingleOrDefault(t => t.Id == finishedSurveyDto.Id);               
            });

            return Ok(finishedSurveyDtos);
        }

        [HttpGet]
        public IActionResult GetAllSurveys()
        {
            var allSurveys = _surveyRepository.GetAll().Where(t => t.IsArchived != true).OrderByDescending(t => t.FinishedDate).ToList();
            var allSurveyDtos = Mapper.Map<List<TestDto>>(allSurveys);

            return Ok(allSurveyDtos);
        }
        public IActionResult GetSurveyById(long id)
        {
            var surveyDto = Mapper.Map<TestDto>(_surveyRepository.GetAll().SingleOrDefault(s => s.Id == id));
            return Ok(surveyDto);
        }

        [HttpGet]
        public IActionResult GetFinishedSurveyById(long id)
        {
            var finishedSurvey = _surveyRepository.GetAll().SingleOrDefault(s => s.Id == id && s.IsFinished == true);

            if (finishedSurvey == null)
                throw new Exception("Finished survey not found");

            var finishedTestDto = Mapper.Map<TestDto>(finishedSurvey);            
            return Ok(finishedTestDto);
        }

        [HttpPost]
        public IActionResult FinishSurvey([FromBody]long id)
        {
            try
            {
                var survey = _surveyRepository.GetSingle(id);
                survey.IsFinished = true;
                survey.FinishedDate = DateTime.UtcNow;
                _surveyRepository.Edit(survey);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }

            return StatusCode(StatusCodes.Status202Accepted);
        }

        [HttpPost]
        public HttpResponseMessage ArchiveSurvey(long id)
        {
            try
            {
                var survey = _surveyRepository.GetSingle(id);
                survey.IsArchived = true;
                _surveyRepository.Edit(survey);
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }

            return new HttpResponseMessage(HttpStatusCode.Accepted);
        }
    }
}
