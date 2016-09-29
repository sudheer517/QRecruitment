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
    public class QuestionsController : ODataController
    {
        private readonly IQuestionRepository _questionRepository;

        public QuestionsController(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }

        // GET api/<controller> 
        //http://localhost:60606/odata/Questions
        [HttpGet]
        [EnableQuery]
        public IHttpActionResult Get()
        {
            var questions = _questionRepository.GetAll().ToList();

            var qDtos = Mapper.Map<IList<QuestionDto>>(questions);
            
            return Ok(qDtos);
        }

        //http://localhost:60606/odata/Questions(1)
        [HttpGet]
        [ODataRoute("Questions({key})")]
        [EnableQuery]
        public IHttpActionResult GetSingle([FromODataUri] int key)
        {
            var question = _questionRepository.GetAll().Single(item => item.Id == key);

            return Ok(Mapper.Map<QuestionDto>(question));
        }

        //http://localhost:60606/odata/Questions
        // For creating
        [HttpPost]
        [ODataRoute("Questions")]
        public IHttpActionResult Post(QuestionDto questionDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var inputQuestion = Mapper.Map<Question>(questionDto);

            var result = _questionRepository.Add(inputQuestion);

            return Created(Mapper.Map<QuestionDto>(result));
        }

        //http://localhost:60606/odata/Questions(3)
        // For full update
        [HttpPut]
        [ODataRoute("Questions({key})")]
        public IHttpActionResult Put([FromODataUri] int key, QuestionDto questionDto)
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
        [ODataRoute("Questions({key})/Text")]
        public IHttpActionResult GetQuestionProperty([FromODataUri] int key)
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

            return this.CreateOKHttpActionResult(propertyValue);
        }

        [HttpGet]
        [ODataRoute("Questions({key})/Options")]
        public IHttpActionResult GetQuestionCollectionProperty([FromODataUri] int key)
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