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
    public class QuestionController : ApiController
    {
        private readonly IQuestionRepository _questionRepository;

        public QuestionController(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
            Mapper.Initialize(cfg => cfg.CreateMap<Question, QuestionDto>());
        }

        // GET api/<controller>
        public IEnumerable<QuestionDto> GetQuestions()
        {
            return _questionRepository.GetAll().Select(question => Mapper.Map<QuestionDto>(question)).ToList();
        }

        // GET api/<controller>/5
        public QuestionDto Get(long id)
        {
            return Mapper.Map<QuestionDto>(_questionRepository.FindById(id));
        }
    }
}