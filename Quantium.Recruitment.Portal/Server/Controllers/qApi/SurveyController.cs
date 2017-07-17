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
using System.Linq.Expressions;
using AspNetCoreSpa.Server;
using OfficeOpenXml.Drawing;
using Microsoft.AspNetCore.Hosting;
using AspNetCoreSpa;
using Newtonsoft.Json;

namespace Quantium.Recruitment.Portal.Server.Controllers.qApi
{
    [Authorize(Roles = "Admin")]
    [Route("[controller]/[action]/{id?}")]
    public class SurveyController : Controller
    {
        private readonly IEntityBaseRepository<SurveyQuestion> _surveyQuestionRepository;
        private readonly IEntityBaseRepository<SurveyResponse> _surveyResponseRepository;
        private readonly IEntityBaseRepository<Candidate> _candidateRepository;

        public SurveyController(
            IEntityBaseRepository<SurveyQuestion> surveyQuestionRepository,
            IEntityBaseRepository<SurveyResponse> surveyResponseRepository,
            IEntityBaseRepository<Candidate> candidateRepository)
        {
            _surveyQuestionRepository = surveyQuestionRepository;
            _surveyResponseRepository = surveyResponseRepository;
            _candidateRepository = candidateRepository;
        }


        // Survey Questions 
        [HttpGet]
        public IActionResult GetAll(bool paging = false, int pageNumber = 1, int questionsPerPage = 10, int labelId = 0, int difficultyId = 0)
        {
            IList<SurveyQuestion> questions = null;

            IList<SurveyQuestion> totalQuestions =
                _surveyQuestionRepository.GetAll().ToList();

            if (paging == true)
            {
                questions =
                    totalQuestions.
                    Skip((pageNumber - 1) * questionsPerPage).
                    Take(questionsPerPage).
                    ToList();
            }
            else
            {
                questions = totalQuestions.ToList();
            }

            var qDtos = Mapper.Map<IList<SurveyQuestionDto>>(questions);

            if (paging == true)
            {
                return Ok(new { totalPages = Math.Ceiling((double)totalQuestions.Count / questionsPerPage), questions = qDtos, totalQuestions = totalQuestions.Count });
            }
            else
            {
                return Ok(qDtos);
            }
        }

        [HttpPost]
        public IActionResult AddQuestions()
        {
            var file = Request.Form.Files[0];

            var fs = file.OpenReadStream();

            var excelPackage = new ExcelPackage(fs);
            fs.Dispose();

            var workSheet = excelPackage.Workbook.Worksheets.FirstOrDefault();
            try
            {
                var questionDtos = GetQuestionDtosFromWorkSheet(workSheet, false);

                foreach (var questionDto in questionDtos)
                {

                    var inputQuestion = Mapper.Map<SurveyQuestion>(questionDto);


                    var result = _surveyQuestionRepository.Add(inputQuestion);
                }

                return Created(string.Empty, "All questions created");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        private IList<SurveyQuestionDto> GetQuestionDtosFromWorkSheet(ExcelWorksheet workSheet, bool isPreview = true)
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

                    //for empty rows
                    if (string.IsNullOrEmpty(questionAndOptions[0].Trim()))
                    {
                        continue;
                    }


                    SurveyQuestionDto newQuestion = new SurveyQuestionDto
                    {
                        Text = questionAndOptions[1].Trim(),
                    };
                    questionDtos.Add(newQuestion);

                }

            }

            return questionDtos;
        }

        [HttpPost]
        public IActionResult MarkQuestionInActive([FromBody]long questionId)
        {
            var question = _surveyQuestionRepository.GetSingle(q => q.Id == questionId);

            try
            {
                _surveyQuestionRepository.Delete(question);
                _surveyQuestionRepository.Commit();
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return Ok();
        }

        // Survey Responses

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]int[] candidateIds)
        {
            string candidatename = this.User.Identity.Name;

            List<SurveyQuestion> questionsToCreate = _surveyQuestionRepository.GetAll().ToList();
            foreach (int CandidateId in candidateIds)
            {
                var res = await _surveyResponseRepository.FindByAsync(a => a.CandidateId == CandidateId);
                if (res.Count > 0)
                    continue;
                foreach (SurveyQuestion question in questionsToCreate)
                {

                    var newResponse = new SurveyResponse
                    {
                        CandidateId = CandidateId,
                        Candidate = _candidateRepository.GetSingle(CandidateId),
                        SurveyQuestionId = question.Id,
                        SurveyQuestion = question,
                        Response = "",

                    };
                    _surveyResponseRepository.Add(newResponse);

                }
            }

            return Ok(JsonConvert.SerializeObject("Created Survey for Candidate Successfully"));
        }

        [HttpGet]
        public IList<CandidateDto> GetSurveydCandidates()
        {           

            var surveyedCandidates = _candidateRepository.GetAll().Where(c => _surveyResponseRepository.GetAll().Select(a => a.CandidateId).Distinct().Contains(c.Id));

            IList<CandidateDto>  resultDtos = Mapper.Map<IList<CandidateDto>>(surveyedCandidates);

            return resultDtos;
        }

        [HttpGet] 
        public IList<SurveyResponseDto> GetSurveyResponses(int candidateId)
        {
           // IList<SurveyResponseDto> resultDtos = new List<SurveyResponseDto>();
            var candidateResponses = _surveyResponseRepository.
                AllIncluding(q =>q.Candidate, q => q.SurveyQuestion).
                Where(a => a.CandidateId == candidateId).ToList();
            //foreach (var response in candidateResponses)
            //{
            //    response.

            //}

            IList<SurveyResponseDto> resultDtos = Mapper.Map<IList<SurveyResponseDto>>(candidateResponses);

            return resultDtos;
        }
  
    }
        
}