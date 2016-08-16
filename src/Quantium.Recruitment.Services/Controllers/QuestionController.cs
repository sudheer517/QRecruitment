using Microsoft.AspNetCore.Mvc;
using Quantium.Recruitment.Infrastructure;
using Quantium.Recruitment.Infrastructure.Repositories;
using Quantium.Recruitment.Services.Models;
using System.Collections.Generic;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Quantium.Recruitment.Services.Controllers
{
    [Route("api/[controller]")]
    public class QuestionController : Controller
    {
        private IQuestionRepository _questionRepository;

        public QuestionController(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }

        // GET: api/values
        [HttpGet]
        public IList<string> GetTemp()
        {
            var data = _questionRepository.GetQestion(1);
            return new List<string> { "hola" };
        }

    }
}
