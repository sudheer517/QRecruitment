using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Quantium.Recruitment.Portal.Models;
using Microsoft.AspNetCore.Identity;
using System.IO;
using Microsoft.Net.Http.Headers;
using Quantium.Recruitment.Portal.Helpers;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Text;
using System.Net;
using Newtonsoft.Json;
using Quantium.Recruitment.ApiServiceModels;
// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Quantium.Recruitment.Portal.Controllers
{
    public class QuestionController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<QRecruitmentRole> _roleManager;
        private readonly IHttpHelper _helper;

        public QuestionController(UserManager<ApplicationUser> userManager, RoleManager<QRecruitmentRole> roleManager, IHttpHelper helper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _helper = helper;
        }

        [HttpPost]
        public IActionResult AddQuestions()
        {
            var file = Request.Form.Files[0];

            string[] contentAsLines = GetContentFromFile(file);

            string[] headers = contentAsLines[0].Split(',');
            //var fileName = file.FileName;
            IList<QuestionDto> questions = new List<QuestionDto>();

            for (int i = 1; i < contentAsLines.Length; i++)
            {
                questions.Add(ParseLineToQuestion(headers, contentAsLines[i]));
            }

            try
            {
                var response = _helper.Post("api/Question/AddQuestions", questions);
            }
            catch (Exception ex)
            {
                return Json(ex);
            }

            return Json("Success");
        }

        private string[] GetContentFromFile(IFormFile file)
        {

            string fileContent;

            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                fileContent = reader.ReadToEnd();
            }

            return fileContent.Split(new string[] { "\r\n" }, StringSplitOptions.None).Where(item => item.Trim().Length > 0).ToArray();
        }

        private QuestionDto ParseLineToQuestion(string[] headers, string questionLine)
        {
            string[] questionAndOptions = questionLine.Split(',');
            string[] selectedOptions = questionAndOptions[2].Split(';');

            QuestionDto newQuestion = new QuestionDto
            {
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

            return newQuestion;
        }

        [HttpGet]
        public IActionResult GetQuestionsByLabelAndDifficulty()
        {
            var response = _helper.GetData("api/Question/GetQuestionsByLabelAndDifficulty");

            if (response.StatusCode != HttpStatusCode.OK)
                throw new Exception("Question retrival failed");

            return Ok(response.Content.ReadAsStringAsync().Result);
        }
    }
}
