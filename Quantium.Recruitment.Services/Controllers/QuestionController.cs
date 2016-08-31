using Microsoft.Practices.Unity;
using Quantium.Recruitment.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Quantium.Recruitment.Services.Controllers
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
            //var admins = _context.Admins.ToList();
            var newData = "Valu2";
            return new string[] { "value1", "value2" };
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