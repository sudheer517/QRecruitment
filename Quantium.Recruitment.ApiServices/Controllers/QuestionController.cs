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
using Quantium.Recruitment.Infrastructure.Unity;
using Microsoft.Practices.Unity;
using System.Web;
using ClosedXML.Excel;
using Excel;
using System.Data;
using System.IO;

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

        private List<QuestionDto> ParseInputQuestionFile()
        {
            var httpRequest = HttpContext.Current.Request;

            var streamResult = Request.Content.ReadAsStreamAsync().Result;

            List<QuestionDto> questionDtos = new List<QuestionDto>();
            using (var ms = new MemoryStream())
            {
                httpRequest.InputStream.CopyToAsync(ms);
                
                IExcelDataReader reader = ExcelReaderFactory.CreateOpenXmlReader(ms);
                DataSet dataset = reader.AsDataSet();
                var count = 1;
                List<string> headers = new List<string>();
                foreach (DataRow item in dataset.Tables[0].Rows)
                {
                    if (count == 1)
                    {
                        item.ItemArray.ForEach(i => headers.Add(i.ToString()));
                    }
                    else
                    {
                        List<string> questionAndOptions = new List<string>();

                        item.ItemArray.ForEach(i => questionAndOptions.Add(i.ToString()));

                        string[] selectedOptions = questionAndOptions[2].Split(';');

                        if (!validateQuestions(questionAndOptions, headers, selectedOptions))
                        {
                            string message = "Id " + questionAndOptions[0] + " has some invalid data";

                            throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, message));
                        }

                        QuestionDto newQuestion = new QuestionDto
                        {
                            Id = Convert.ToInt64(questionAndOptions[0]),
                            Text = questionAndOptions[1],
                            TimeInSeconds = Convert.ToInt32(questionAndOptions[3]),
                            Label = new LabelDto { Name = questionAndOptions[10] },
                            Difficulty = new DifficultyDto { Name = questionAndOptions[11] },
                            RandomizeOptions = Convert.ToBoolean(questionAndOptions[12]),
                            ImageUrl = questionAndOptions[13],
                            QuestionGroup = new QuestionGroupDto
                            {
                                Description = questionAndOptions[14]
                            },
                            Options = new List<OptionDto>
                            {
                                new OptionDto
                                {
                                    Text = questionAndOptions[4],
                                    IsAnswer = selectedOptions.Contains(headers[4])
                                },
                                new OptionDto
                                {
                                    Text = questionAndOptions[5],
                                    IsAnswer = selectedOptions.Contains(headers[5])
                                },
                                new OptionDto
                                {
                                    Text = questionAndOptions[6],
                                    IsAnswer = selectedOptions.Contains(headers[6])
                                },
                                new OptionDto
                                {
                                    Text = questionAndOptions[7],
                                    IsAnswer = selectedOptions.Contains(headers[7])
                                },
                                new OptionDto
                                {
                                    Text = questionAndOptions[8],
                                    IsAnswer = selectedOptions.Contains(headers[8])
                                },
                                new OptionDto
                                {
                                    Text = questionAndOptions[9],
                                    IsAnswer = selectedOptions.Contains(headers[9])
                                }
                            }
                        };

                        questionDtos.Add(newQuestion);
                    }
                    count++;
                }
            }

            return questionDtos;
        }

        [HttpPost]
        public IHttpActionResult PreviewQuestions()
        {
            var questionDtos = ParseInputQuestionFile();
            return Ok(questionDtos);
        }

        [HttpPost]
        public IHttpActionResult AddQuestions()
        {
            var questionDtos = ParseInputQuestionFile();

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

                    if (label != null)
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
            catch (Exception ex)
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

        private bool validateQuestions(IList<string> question, IList<string> headers, IList<string> options)
        {
            var mandatoryFields = new List<int> { 1, 2, 3, 10, 11 };
            var dataValid = true;

            foreach (int mandatory in mandatoryFields)
            {
                if (question.ElementAt(mandatory) == string.Empty)
                {
                    dataValid = false;
                }
            }

            var optionFields = new List<int> { 4, 5, 6, 7, 8, 9 };

            foreach (string option in options)
            {
                if (!(optionFields.Any(field => headers.ElementAt(field) == option)))
                {
                    dataValid = false;
                }
            }

            if (optionFields.All(field => question.ElementAt(field) == string.Empty))
            {
                dataValid = false;
            }

            return dataValid;
        }
    }
}