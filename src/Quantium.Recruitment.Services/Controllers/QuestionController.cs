using Microsoft.AspNetCore.Mvc;
using Quantium.Recruitment.Services.Models;
using Quantium.Recruitment.Services.Repositories;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Quantium.Recruitment.Services.Controllers
{
    [Route("api/[controller]")]
    public class QuestionController : Controller
    {
        private IQuestionRepository _questionRepo;

        public QuestionController(IQuestionRepository questionRepo)
        {
            _questionRepo = questionRepo;
        }

        [HttpGet]
        public void AddQuestion(QuestionDto questionDto)
        {
            _questionRepo.AddQuestion(questionDto);
        }

        [HttpGet("{questionId}")]
        public QuestionDto GetQuestion(long questionId)
        {
            return _questionRepo.GetQestion(questionId);
        }
    }
}
