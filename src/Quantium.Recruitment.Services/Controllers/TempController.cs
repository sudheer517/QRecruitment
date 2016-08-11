using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Quantium.Recruitment.Services.Models;
using Quantium.Recruitment.Infrastructure;

namespace Quantium.Recruitment.Services.Controllers
{
    [Route("api/[controller]")]
    public class TempController : Controller
    {
        private RecruitmentContext _context;

        public TempController(RecruitmentContext context)
        {
            _context = context;
        }
        public static IList<Temp> TempRepo = new List<Temp>
        {
            new Temp { Id = 1, QuestionText = "Sample Question Text 1", Answer="Answer1" },
            new Temp { Id = 2, QuestionText = "Sample Question Text 2", Answer="Answer2" },
            new Temp { Id = 3, QuestionText = "Sample Question Text 3", Answer="Answer3" },
            new Temp { Id = 4, QuestionText = "Sample Question Text 4", Answer="Answer4" },
            new Temp { Id = 5, QuestionText = "Sample Question Text 5", Answer="Answer5" },
            new Temp { Id = 6, QuestionText = "Sample Question Text 6", Answer="Answer6" },
            new Temp { Id = 7, QuestionText = "Sample Question Text 7", Answer="Answer7" },
            new Temp { Id = 8, QuestionText = "Sample Question Text 8", Answer="Answer8" },
        };

        // GET: api/values
        [HttpGet]
        public IEnumerable<Temp> GetTemp()
        {
            var data = _context.Admins.ToList();
            return TempRepo;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var question = TempRepo.SingleOrDefault(q => q.Id == id);

            if (question != null)
            {
                return Ok(question);
            }
            else
            {
                return new NotFoundObjectResult(id);
            }
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]Temp question)
        {
            var questionSearchResult = TempRepo.SingleOrDefault(q => q.Id == question.Id);

            if (questionSearchResult == null)
            {
                TempRepo.Add(question);
            }
            else
            {
                questionSearchResult.QuestionText = question.QuestionText;
                questionSearchResult.Answer = question.Answer;
            }

        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]Temp question)
        {
            var questionSearchResult = TempRepo.SingleOrDefault(q => q.Id == question.Id);

            if (questionSearchResult == null)
            {
                TempRepo.Add(question);
            }
            else
            {
                questionSearchResult.Id = id;
                questionSearchResult.QuestionText = question.QuestionText;
                questionSearchResult.Answer = question.Answer;
            }
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var question = TempRepo.SingleOrDefault(q => q.Id == id);

            if (question != null)
            {
                TempRepo.Remove(question);
            }
        }
    }
}
