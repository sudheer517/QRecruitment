using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.IO;
using System.Net;
using Quantium.Recruitment.Portal.Helpers;
using Microsoft.AspNetCore.Http;
using Quantium.Recruitment.Models;
using OfficeOpenXml;
using AspNetCoreSpa.Server.Repositories.Abstract;
using Quantium.Recruitment.Entities;
using AutoMapper;
using Quantium.Recruitment.Portal.Server.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace Quantium.Recruitment.Portal.Server.Controllers.qApi
{
    [Authorize(Roles = "Admin")]
    [Route("[controller]/[action]/{id?}")]
    public class SurveyQuestionController : Controller
    {
        private readonly IHttpHelper _helper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEntityBaseRepository<SurveyQuestion> _surveyQuestionRepository;

        public SurveyQuestionController(IHttpHelper helper, IHttpContextAccessor httpContentAccessor, IEntityBaseRepository<SurveyQuestion> surveyQuestionRepository)
        {
            _helper = helper;
            _httpContextAccessor = httpContentAccessor;
            _surveyQuestionRepository = surveyQuestionRepository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var surveyQuestions = _surveyQuestionRepository.GetAll();
            var surveyQuestionDtos = Mapper.Map<IList<SurveyQuestionDto>>(surveyQuestions);
            return Ok(surveyQuestionDtos);
        }
        
    }
}
