﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using Quantium.Recruitment.ApiServices.Models;
using Quantium.Recruitment.Entities;
using Quantium.Recruitment.Infrastructure.Repositories;
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

                    var label = _labelRepository.FindByName(questionDto.Label.Name);

                    var difficulty = _difficultyRepository.FindByName(questionDto.Difficulty.Name);

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

                    if(label != null)
                    {
                        inputQuestion.Label = label;
                    }
                    else
                    {
                        inputQuestion.Label = null;
                        inputQuestion.LabelId = null;
                    }

                    if (difficulty != null)
                    {
                        inputQuestion.Difficulty = difficulty;
                    }
                    else
                    {
                        inputQuestion.Difficulty = null;
                        inputQuestion.DifficultyId = null;
                    }

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
        public IHttpActionResult GetQuestionsByLabelAndDifficulty()
        {
            var allQuestions = _questionRepository.GetAll();

            var questionDifficultyLabelDto = allQuestions.GroupBy(x => new { x.LabelId, x.DifficultyId}, (key, group) => new Question_Difficulty_LabelDto
            {
                LabelId = key.LabelId.Value,
                DifficultyId = key.DifficultyId.Value,
                QuestionCount = group.ToList().Count
            }).ToList();

            if(questionDifficultyLabelDto.Count < 1)
            {
                throw new Exception("No questions found");
            }

            return Ok(questionDifficultyLabelDto);
        }
    }
}