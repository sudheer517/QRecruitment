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
    public class QuestionController : Controller
    {
        private readonly IHttpHelper _helper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEntityBaseRepository<Question> _questionRepository;
        private readonly IEntityBaseRepository<Label> _labelRepository;
        private readonly IEntityBaseRepository<Difficulty> _difficultyRepository;
        private readonly IEntityBaseRepository<QuestionGroup> _questionGroupRepository;

        public QuestionController(IHttpHelper helper, IHttpContextAccessor httpContextAccessor,
            IEntityBaseRepository<Question> questionRepository,
            IEntityBaseRepository<Label> labelRepository,
            IEntityBaseRepository<Difficulty> difficultyRepository,
            IEntityBaseRepository<QuestionGroup> questionGroupRepository)
        {
            _helper = helper;
            _httpContextAccessor = httpContextAccessor;
            _questionRepository = questionRepository;
            _labelRepository = labelRepository;
            _difficultyRepository = difficultyRepository;
            _questionGroupRepository = questionGroupRepository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var questions = _questionRepository.AllIncluding(q => q.Label, q => q.Difficulty, q => q.Options, q => q.QuestionGroup).Where(q => q.IsActive != false);

            var qDtos = Mapper.Map<IList<QuestionDto>>(questions);

            return Ok(qDtos);
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
                var questionDtos = GetQuestionDtosFromWorkSheet(workSheet);

                foreach (var questionDto in questionDtos)
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    questionDto.Options = questionDto.Options.Where(item => item.Text.Trim().Length > 0).ToList();

                    Action<Object, Object> removeQuestionDtoId =
                        (qDto, obj) => ((QuestionDto)qDto).Id = 0;

                    var inputQuestion = Mapper.Map<Question>(questionDto, opts => opts.BeforeMap(removeQuestionDtoId));

                    string userEnteredLabel = questionDto.Label.Name.Trim();
                    string labelName = "Others";
                    if (!string.IsNullOrEmpty(userEnteredLabel))
                    {
                        labelName = userEnteredLabel;
                    }
                    var label = _labelRepository.GetSingle(item => item.Name == labelName);
                    inputQuestion.Label = label;
                    if (label == null)
                    {
                        var newLabel = _labelRepository.Add(new Label { Name = labelName });
                        inputQuestion.Label = newLabel;
                        inputQuestion.LabelId = newLabel.Id;
                    }

                    string userEnteredDifficulty = questionDto.Difficulty.Name.Trim();
                    string difficultyName = "Easy";
                    if (!string.IsNullOrEmpty(userEnteredDifficulty))
                    {
                        difficultyName = userEnteredDifficulty;
                    }
                    var difficulty = _difficultyRepository.GetSingle(item => item.Name == difficultyName);
                    inputQuestion.Difficulty = difficulty;
                    if (difficulty == null)
                    {
                        var newDifficulty = _difficultyRepository.Add(new Difficulty { Name = questionDto.Difficulty.Name });
                        inputQuestion.Difficulty = newDifficulty;
                        inputQuestion.DifficultyId = newDifficulty.Id;
                    }

                    if (!string.IsNullOrEmpty(questionDto.QuestionGroup.Description))
                    {
                        var questionGroup = _questionGroupRepository.GetSingle(item => item.Description == questionDto.QuestionGroup.Description);

                        if (questionGroup != null)
                            inputQuestion.QuestionGroup = questionGroup;
                    }
                    else
                    {
                        inputQuestion.QuestionGroup = null;
                        inputQuestion.QuestionGroupId = null;
                    }

                    inputQuestion.IsActive = true;
                    var result = _questionRepository.Add(inputQuestion);
                }

                return Created(string.Empty, "All questions created");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult PreviewQuestions(ICollection<IFormFile> files)
        {
            var file = Request.Form.Files[0];

            var fs = file.OpenReadStream();

            var excelPackage = new ExcelPackage(fs);
            fs.Dispose();

            var workSheet = excelPackage.Workbook.Worksheets.FirstOrDefault();
            try
            {
                var questionDtos = GetQuestionDtosFromWorkSheet(workSheet);
                return Ok(questionDtos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        private IList<QuestionDto> GetQuestionDtosFromWorkSheet(ExcelWorksheet workSheet)
        {

            var row = workSheet.Dimension.Start.Row;
            var end = workSheet.Dimension.End.Row;

            IList<string> headers = new List<string>();

            IList<QuestionDto> questionDtos = new List<QuestionDto>();

            for (int rowIndex = workSheet.Dimension.Start.Row; rowIndex <= workSheet.Dimension.End.Row; rowIndex++)
            {
                if (rowIndex == 1)
                {
                    headers = ExcelHelper.GetExcelHeaders(workSheet, rowIndex);
                }
                else
                {
                    IList<string> questionAndOptions = ExcelHelper.GetExcelHeaders(workSheet, rowIndex);

                    string[] selectedOptions = questionAndOptions[2].Split(';').Select(item => item.Trim()).ToArray();

                    if (!validateQuestions(questionAndOptions, headers, selectedOptions))
                    {
                        string message = "Id " + questionAndOptions[0] + " has some invalid data";

                        throw new Exception(message);
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
                        IsRadio = !string.IsNullOrEmpty(questionAndOptions[15]) ? Convert.ToBoolean(questionAndOptions[15]) : false,
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

                    var optionsList = new List<OptionDto>();

                    for (int i = 4; i < 10; i++)
                    {
                        string questionText = questionAndOptions[i].Trim();
                        if (!string.IsNullOrEmpty(questionText))
                        {
                            optionsList.Add(new OptionDto
                            {
                                Text = questionText,
                                IsAnswer = selectedOptions.Contains(headers[i])
                            });
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (optionsList.Count == 0)
                    {
                        continue;
                    }
                    else
                    {
                        newQuestion.Options = optionsList;
                    }

                    questionDtos.Add(newQuestion);

                }

            }

            return questionDtos;
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

        [HttpGet]
        //[Route("/GetQuestionsByLabelAndDifficulty")]
        public IActionResult GetQuestionsByLabelAndDifficulty()
        {
            var allQuestions = _questionRepository.GetAll().Where(q => q.IsActive != false);

            var questionDifficultyLabelDto = 
                allQuestions.GroupBy(x => new { x.LabelId, x.DifficultyId }, (key, group) => new Question_Difficulty_LabelDto
            {
                LabelId = key.LabelId.Value,
                DifficultyId = key.DifficultyId.Value,
                QuestionCount = group.ToList().Count
            }).ToList();

            if (questionDifficultyLabelDto.Count < 1)
            {
                return NotFound();
            }

            return Ok(questionDifficultyLabelDto);
        }

        [HttpPost]
        public IActionResult MarkQuestionInActive([FromBody]long questionId)
        {
            var question = _questionRepository.GetSingle(q => q.Id == questionId);

            try
            {
                question.IsActive = false;
                _questionRepository.Edit(question);
                _questionRepository.Commit();
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
        
}