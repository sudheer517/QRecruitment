using Quantium.Recruitment.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Quantium.Recruitment.ApiServices.Controllers
{
    public class QuestionController : ApiController
    {
        private IRecruitmentContext _context;

        public QuestionController(IRecruitmentContext context)
        {
            _context = context;
        }

        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return _context.Questions.Select(q => q.Text);
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}