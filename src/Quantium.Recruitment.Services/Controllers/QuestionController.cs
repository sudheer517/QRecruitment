using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Quantium.Recruitment.Infrastructure;
using Quantium.Recruitment.Infrastructure.Repositories;
using Quantium.Recruitment.Services.Models;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata;
using System.Threading.Tasks;

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
            return new List<string> { "hola" };
        }

        [HttpPost]
        public IList<string> Post(ICollection<IFormFile> files)
        {
            var file = Request.Form.Files[0];

            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                var fileContent = reader.ReadToEnd();
                var parsedContentDisposition = ContentDispositionHeaderValue.Parse(file.ContentDisposition);
                var fileName = parsedContentDisposition.FileName;
            }

            return new List<string>{ "success" };
        }
    }
}
