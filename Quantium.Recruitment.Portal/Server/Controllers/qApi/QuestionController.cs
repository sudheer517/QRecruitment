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
        private readonly IEntityBaseRepository<Admin> _adminRepository;
        private readonly IHostingEnvironment _env;

        public QuestionController(IHttpHelper helper, IHttpContextAccessor httpContextAccessor,
            IEntityBaseRepository<Question> questionRepository,
            IEntityBaseRepository<Label> labelRepository,
            IEntityBaseRepository<Difficulty> difficultyRepository,
            IEntityBaseRepository<QuestionGroup> questionGroupRepository,
            IEntityBaseRepository<Admin> adminRepository,
            IHostingEnvironment env)
        {
            _helper = helper;
            _httpContextAccessor = httpContextAccessor;
            _questionRepository = questionRepository;
            _labelRepository = labelRepository;
            _difficultyRepository = difficultyRepository;
            _questionGroupRepository = questionGroupRepository;
            _adminRepository = adminRepository;
            _env = env;
        }

        [HttpGet]
        public IActionResult GetAll(bool paging = false, int pageNumber = 1, int questionsPerPage = 10, int labelId = 0, int difficultyId = 0)
        {
            IList<Question> questions = null;

            Expression<Func<Question, bool>> predicate = q => q.IsActive != false;

            if(labelId != 0)
            {
                Expression<Func<Question, bool>> labelPredicate = q => q.Label.Id == labelId;
                predicate = PredicateHelper.CombineWithAnd(predicate, labelPredicate);
            }

            if (difficultyId != 0)
            {
                Expression<Func<Question, bool>> difficultyPredicate = q => q.Difficulty.Id == difficultyId;
                predicate = PredicateHelper.CombineWithAnd(predicate, difficultyPredicate);
            }

            IList<Question> totalQuestions =
                _questionRepository.
                AllIncluding(q => q.Label, q => q.Difficulty, q => q.Options, q => q.QuestionGroup).
                Where(predicate).
                OrderByDescending(q => q.CreatedUtc).ToList();

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

            var qDtos = Mapper.Map<IList<QuestionDto>>(questions);

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
        public async Task<IActionResult> AddQuestions()
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
                    //Cannot find duplication questions with question text for questions like "what is output of this program". So commenting this out
                    //var duplicateQuestionExists = await _questionRepository.GetSingleAsync(q => q.Text == questionDto.Text.Trim());

                    //if(duplicateQuestionExists != null)
                    //{
                    //    return StatusCode(StatusCodes.Status409Conflict, questionDto.Id);
                    //}

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
                    var label = await _labelRepository.GetSingleAsync(item => item.Name == labelName);
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
                    var difficulty = await _difficultyRepository.GetSingleAsync(item => item.Name == difficultyName);
                    inputQuestion.Difficulty = difficulty;
                    if (difficulty == null)
                    {
                        var newDifficulty = _difficultyRepository.Add(new Difficulty { Name = questionDto.Difficulty.Name });
                        inputQuestion.Difficulty = newDifficulty;
                        inputQuestion.DifficultyId = newDifficulty.Id;
                    }

                    if (!string.IsNullOrEmpty(questionDto.QuestionGroup.Description))
                    {
                        var questionGroup = await _questionGroupRepository.GetSingleAsync(item => item.Description == questionDto.QuestionGroup.Description);

                        if (questionGroup != null)
                            inputQuestion.QuestionGroup = questionGroup;
                    }
                    else
                    {
                        inputQuestion.QuestionGroup = null;
                        inputQuestion.QuestionGroupId = null;
                    }
                    var adminEmail = this.User.Identities.First().Name;
                    var admin = await _adminRepository.GetSingleAsync(a => a.Email == adminEmail);
                    inputQuestion.CreatedByUserId = admin.Id;
                    inputQuestion.IsActive = true;
                    inputQuestion.CreatedUtc = DateTime.UtcNow;
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
        public async Task<IActionResult> PreviewQuestions()
        {
            var file = Request.Form.Files[0];

            var fs = file.OpenReadStream();

            var excelPackage = new ExcelPackage(fs);
            fs.Dispose();

            var workSheet = excelPackage.Workbook.Worksheets.FirstOrDefault();
            try
            {
                var questionDtos = GetQuestionDtosFromWorkSheet(workSheet);

                //Cannot find duplication questions with question text for questions like "what is output of this program". So commenting this out
                //foreach (var questionDto in questionDtos)
                //{
                //    var duplicateQuestionExists = await _questionRepository.GetSingleAsync(q => q.Text == questionDto.Text.Trim());

                //    if (duplicateQuestionExists != null)
                //    {
                //        return StatusCode(StatusCodes.Status409Conflict, questionDto.Id);
                //    }
                //}
                
                return Ok(questionDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status406NotAcceptable, ex.Message);
            }

        }

        private IDictionary<int, string> CreateImages(ExcelWorksheet workSheet, bool isPreview)
        {
            var imageCount = workSheet.Drawings.Count;

            var adminEmail = this.User.Identities.First().Name;

            string folderToSave = "QuestionImagesPreviewStorePath";

            if (!isPreview)
            {
                string previewDirectoryPath = $"{_env.WebRootPath}{Startup.Configuration[folderToSave]}{adminEmail}\\";
                Directory.Delete(previewDirectoryPath, true);
                folderToSave = "QuestionImagesStorePath";
            }

            var directoryPath = $"{_env.WebRootPath}{Startup.Configuration[folderToSave]}{adminEmail}\\";

            Directory.CreateDirectory(directoryPath);

            IDictionary<int, string> imageRowPathMap = new Dictionary<int, string>();

            for (int imageIndex = 0; imageIndex < imageCount; imageIndex++)
            {
                var image = workSheet.Drawings[imageIndex] as ExcelPicture;
                var imageCol = image.From.Column;
                var imageRow = image.From.Row;

                var fileName = $"{DateTime.Now.ToString("yyyyMMddTHHmmss-FFF")}-{imageCol}-{imageRow}.{image.ImageFormat.ToString().ToLower()}";
                var pathWithFileName = directoryPath + fileName;

                image.Image.Save(pathWithFileName);

                imageRowPathMap.Add(imageRow + 1, $"{Startup.Configuration[folderToSave]}{adminEmail}\\{fileName}");
            }

            return imageRowPathMap;
        }

        private IList<QuestionDto> GetQuestionDtosFromWorkSheet(ExcelWorksheet workSheet, bool isPreview = true)
        {

            var row = workSheet.Dimension.Start.Row;
            var end = workSheet.Dimension.End.Row;

            IDictionary<int, string> imagePathMap = CreateImages(workSheet, isPreview);

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

                    //for empty rows
                    if (string.IsNullOrEmpty(questionAndOptions[0].Trim()))
                    {
                        continue;
                    }

                    string[] selectedOptions = questionAndOptions[2].Split(';').Select(item => item.Trim()).ToArray();

                    if (!validateQuestions(questionAndOptions, headers, selectedOptions))
                    {
                        string message = "Id " + questionAndOptions[0] + " has some invalid data";

                        throw new Exception(message);
                    }

                    string imagePath = string.Empty;
                    imagePathMap.TryGetValue(rowIndex, out imagePath);

                    QuestionDto newQuestion = new QuestionDto
                    {
                        Id = Convert.ToInt64(questionAndOptions[0]),
                        Text = questionAndOptions[1].Trim(),
                        TimeInSeconds = Convert.ToInt32(questionAndOptions[3].Trim()),
                        Label = new LabelDto { Name = questionAndOptions[10].Trim() },
                        Difficulty = new DifficultyDto { Name = questionAndOptions[11].Trim() },
                        RandomizeOptions = !string.IsNullOrEmpty(questionAndOptions[12].Trim()) ? Convert.ToBoolean(questionAndOptions[12]) : true,
                        ImageUrl = imagePath,
                        QuestionGroup = new QuestionGroupDto
                        {
                            Description = questionAndOptions[14].Trim()
                        },
                        IsRadio = !string.IsNullOrEmpty(questionAndOptions[15].Trim()) ? Convert.ToBoolean(questionAndOptions[15]) : false,
                        Options = new List<OptionDto>
                            {
                                new OptionDto
                                {
                                    Text = questionAndOptions[4].Trim(),
                                    IsAnswer = selectedOptions.Contains(headers[4])
                                },
                                new OptionDto
                                {
                                    Text = questionAndOptions[5].Trim(),
                                    IsAnswer = selectedOptions.Contains(headers[5])
                                },
                                new OptionDto
                                {
                                    Text = questionAndOptions[6].Trim(),
                                    IsAnswer = selectedOptions.Contains(headers[6])
                                },
                                new OptionDto
                                {
                                    Text = questionAndOptions[7].Trim(),
                                    IsAnswer = selectedOptions.Contains(headers[7])
                                },
                                new OptionDto
                                {
                                    Text = questionAndOptions[8].Trim(),
                                    IsAnswer = selectedOptions.Contains(headers[8])
                                },
                                new OptionDto
                                {
                                    Text = questionAndOptions[9].Trim(),
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