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
using Microsoft.AspNetCore.Http;

namespace Quantium.Recruitment.ApiServices.Controllers
{
    [Route("api/[controller]/[action]/{id?}")]
    public class SurveyController : Controller
    {
        private readonly ISurveyRepository _surveyRepository;
        private readonly ISurveyChallengeRepository _surveyChallengeRepository;
        private readonly ISurveyQuestionRepository _surveyQuestionRepository;

        public SurveyController(
            ISurveyRepository surveyRepository, 
            ISurveyChallengeRepository surveyChallengeRepository, 
            ISurveyQuestionRepository surveyQuestionRepository)
        {
            _surveyRepository = surveyRepository;
            _surveyChallengeRepository = surveyChallengeRepository;
            _surveyQuestionRepository = surveyQuestionRepository;
        }

        [HttpPost]
        public IActionResult GenerateSurveys([FromBody]List<CandidateDto> candidatesDto)
        {
            return StatusCode(StatusCodes.Status201Created);
        }
    }

}
