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
using Quantium.Recruitment.Infrastructure.Unity;
using Microsoft.Practices.Unity;

namespace Quantium.Recruitment.ApiServices.Controllers
{
    //[Authorize]
    public class QuestionController : ApiController
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly ILabelRepository _labelRepository;
        private readonly IDifficultyRepository _difficultyRepository;
        private readonly IUnityContainer _container;
        private readonly IQuestionGroupRepository _questionGroupRepository;
        public QuestionController(
            IQuestionRepository questionRepository,
            ILabelRepository labelRepository,
            IDifficultyRepository difficultyRepository, 
            IUnityContainer container,
            IQuestionGroupRepository questionGroupRepository)
        {
            _questionRepository = questionRepository;
            _labelRepository = labelRepository;
            _difficultyRepository = difficultyRepository;
            _container = container;
            _questionGroupRepository = questionGroupRepository;
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

                    questionDto.Options = questionDto.Options.Where(item => item.Text.Trim().Length > 0).ToList();

                    var inputQuestion = Mapper.Map<Question>(questionDto);

                    var label = _labelRepository.FindByName(questionDto.Label);

                    var difficulty = _difficultyRepository.FindByName(questionDto.Difficulty);

                    if (!string.IsNullOrEmpty(questionDto.QuestionGroup.Description))
                    {
                        var questionGroup = _questionGroupRepository.FindByName(questionDto.QuestionGroup.Description);

                        if (questionGroup != null)
                            inputQuestion.QuestionGroup = questionGroup;
                    }
                    else
                    {
                        inputQuestion.QuestionGroup = null;
                        inputQuestion.QuestionGroupId = null;
                    }

                    var questionLabelDifficultyRepository = _container.Resolve<IQuestionLabelDifficultyRepository>();
                    var questionLabelDifficulty = new Question_Label_Difficulty()
                    {
                        Difficulty = difficulty,
                        Label = label,
                        Question = inputQuestion
                    };

                    inputQuestion.DifficultyLabels = new List<Question_Label_Difficulty> { questionLabelDifficulty };


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