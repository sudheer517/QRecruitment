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


        [HttpPost]
        public IActionResult AddSurveyQuestions()
        {
            var file = Request.Form.Files[0];

            var fs = file.OpenReadStream();

            var excelPackage = new ExcelPackage(fs);
            fs.Dispose();

            var workSheet = excelPackage.Workbook.Worksheets.FirstOrDefault();
            try
            {
                var surveyQuestionDtos = GetQuestionDtosFromWorkSheet(workSheet);

                foreach (var surveyQuestionDto in surveyQuestionDtos)
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }
                    
                    Action<Object, Object> removeQuestionDtoId =
                        (qDto, obj) => ((QuestionDto)qDto).Id = 0;

                    var inputQuestion = Mapper.Map<SurveyQuestion>(surveyQuestionDto);


                    var result = _surveyQuestionRepository.Add(inputQuestion);
                }

                return Created(string.Empty, "All questions created");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private IList<SurveyQuestionDto> GetQuestionDtosFromWorkSheet(ExcelWorksheet workSheet)
        {

            var row = workSheet.Dimension.Start.Row;
            var end = workSheet.Dimension.End.Row;

            IList<string> headers = new List<string>();

            IList<SurveyQuestionDto> questionDtos = new List<SurveyQuestionDto>();

            for (int rowIndex = workSheet.Dimension.Start.Row; rowIndex <= workSheet.Dimension.End.Row; rowIndex++)
            {
                if (rowIndex == 1)
                {
                    headers = ExcelHelper.GetExcelHeaders(workSheet, rowIndex);
                }
                else
                {
                    IList<string> questionAndOptions = ExcelHelper.GetExcelHeaders(workSheet, rowIndex);

                    if (!validateQuestions(questionAndOptions))
                    {
                        string message = "Id " + questionAndOptions[0] + " has some invalid data";

                        throw new Exception(message);
                    }

                    SurveyQuestionDto newQuestion = new SurveyQuestionDto
                    {
                        Id = Convert.ToInt64(questionAndOptions[0]),
                        Text = questionAndOptions[1]                        
                    };

                    questionDtos.Add(newQuestion);

                }

            }

            return questionDtos;
        }
        private bool validateQuestions(IList<string> question)
        {
            var mandatoryFields = new List<int> { 1, 2 };
            var dataValid = true;

            foreach (int mandatory in mandatoryFields)
            {
                if (question.ElementAt(mandatory) == string.Empty)
                {
                    dataValid = false;
                }
            }            
            return dataValid;
        }
    }
}
