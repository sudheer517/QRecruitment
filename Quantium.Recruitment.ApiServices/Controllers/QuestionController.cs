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
using Quantium.Recruitment.ApiServices.Helpers;
using System.Web.OData;
using System.Web.OData.Routing;

namespace Quantium.Recruitment.ApiServices.Controllers
{
    //[Authorize]
    public class QuestionController : ApiController
    {
        private readonly IQuestionRepository _questionRepository;

        public QuestionController(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }

        [HttpGet]
        public IHttpActionResult Get()
        {
            var questions = _questionRepository.GetAll().ToList();

            var qDtos = Mapper.Map<IList<QuestionDto>>(questions);
            
            return Ok(qDtos);
        }

        [HttpGet]
        public IHttpActionResult GetSingle(int key)
        {
            var question = _questionRepository.GetAll().Single(item => item.Id == key);

            return Ok(Mapper.Map<QuestionDto>(question));
        }

        [HttpPost]
        public IHttpActionResult AddQuestions([FromBody]List<QuestionDto> questionDtos)
        {
            try
            {
                foreach (var questionDto in questionDtos)
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    var inputQuestion = Mapper.Map<Question>(questionDto);

                    var result = _questionRepository.Add(inputQuestion);
                }
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }

            return Created(string.Empty, "All questions created");
        }

        [HttpPut]
        public IHttpActionResult Put(int key, QuestionDto questionDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dynamicQuestion = _questionRepository.FindById(key);

            if (dynamicQuestion == null)
                return NotFound();

            var updatedQuestion = Mapper.Map(questionDto, dynamicQuestion);
            for (int i = 0; i < dynamicQuestion.Options.Count(); i++)
            {
                Mapper.Map(questionDto.Options[i], dynamicQuestion.Options[i]);
            }

            _questionRepository.Update(updatedQuestion);

            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpGet]
        public IHttpActionResult GetQuestionProperty(int key)
        {
            var question = _questionRepository.FindById(key);

            if (question == null)
                return NotFound();

            var propertyToGet = Url.Request.RequestUri.Segments.Last();

            if (!question.HasProperty(propertyToGet))
                return NotFound();

            var propertyValue = question.GetValue(propertyToGet);

            if (propertyValue == null)
                return StatusCode(HttpStatusCode.NoContent);

            return Ok(propertyValue);
        }

        [HttpGet]
        public IHttpActionResult GetQuestionCollectionProperty(int key)
        {
            var propertyToGet = Url.Request.RequestUri.Segments.Last();

            var question = _questionRepository.FindById(key);

            if (question == null)
                return NotFound();

            var propertyValue = question.Options;

            if (propertyValue == null)
                return StatusCode(HttpStatusCode.NoContent);

            return Ok(Mapper.Map<IList<OptionDto>>(propertyValue));
        }
    }
}