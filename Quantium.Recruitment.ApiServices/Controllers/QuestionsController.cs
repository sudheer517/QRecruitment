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
using System.Web.OData;
using System.Web.OData.Routing;

namespace Quantium.Recruitment.ApiServices.Controllers
{
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

        // GET api/<controller>/5
        //public QuestionDto Get(long id)
        //{
        //    //return Mapper.Map<QuestionDto>(_questionRepository.FindById(id));
        //}
    }
}