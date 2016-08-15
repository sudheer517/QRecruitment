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
        public class TempOption
        {
            public int OptionId { get; set; }
            public string OptionText { get; set; }
        }

        public static IList<Temp> TempRepo = new List<Temp>
        {
            new Temp { Id = 1, QuestionText = "Who is Ned stark's born son?", Options = new List<TempOption> {
                new TempOption { OptionText= "Rob", OptionId=25 },
                new TempOption { OptionText= "Aryan", OptionId=26 },
                new TempOption { OptionText= "Malfoy", OptionId=27 },
                new TempOption { OptionText= "Lucifer", OptionId=28 }}},

            new Temp { Id = 2, QuestionText = "What house has the bear sigil?", Options = new List<TempOption> {
                new TempOption { OptionText= "Stark", OptionId=29 },
                new TempOption { OptionText= "Targaryen", OptionId=30 },
                new TempOption { OptionText= "Mormont", OptionId=31 },
                new TempOption { OptionText= "Tyrell", OptionId=32 }}},

            new Temp { Id = 3, QuestionText = "Where is little finger from?", Options = new List<TempOption> {
                new TempOption { OptionText= "Little", OptionId=33 },
                new TempOption { OptionText= "Vale", OptionId=34 },
                new TempOption { OptionText= "Fingers", OptionId=35 },
                new TempOption { OptionText= "Riverrun", OptionId=36 }}},

            new Temp { Id = 4, QuestionText = "Where did sam go ?", Options = new List<TempOption> {
                new TempOption { OptionText= "White & Black ", OptionId=37 },
                new TempOption { OptionText= "Bravos", OptionId=38 },
                new TempOption { OptionText= "Citadel", OptionId=39 },
                new TempOption { OptionText= "Library of bravos", OptionId=40 }}},

            new Temp { Id = 5, QuestionText = "Who among the following is an old god?", Options = new List<TempOption> {
                new TempOption { OptionText= "Drowned god", OptionId=41 },
                new TempOption { OptionText= "Lord of light", OptionId=42 },
                new TempOption { OptionText= "Amun ra", OptionId=43 },
                new TempOption { OptionText= "Baelor", OptionId=44 }}},

            new Temp { Id = 6, QuestionText = "whos the first red haired seen in got?", Options = new List<TempOption> {
                new TempOption { OptionText= "Red woman", OptionId=45 },
                new TempOption { OptionText= "Ros", OptionId=46 },
                new TempOption { OptionText= "Sansa", OptionId=47 },
                new TempOption { OptionText= "Lyanna", OptionId=48 }}}
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
        public void Post([FromBody]List<int> question)
        {
            //var questionSearchResult = TempRepo.SingleOrDefault(q => q.Id == question.Id);

            //if (questionSearchResult == null)
            //{
            //    TempRepo.Add(question);
            //}
            //else
            //{
            //    questionSearchResult.QuestionText = question.QuestionText;
            //}

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
