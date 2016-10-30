using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Quantium.Recruitment.Portal.Helpers;
using Quantium.Recruitment.ApiServiceModels;
using System.Web.Http;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Quantium.Recruitment.Portal.Controllers
{
    public class TestController : Controller
    {
        private readonly IHttpHelper _helper;

        public TestController(IHttpHelper helper)
        {
            _helper = helper;
        }

        [HttpGet]
        public IActionResult GetNextChallenge()
        {
            var userEmail = this.User.Identities.First().Name;

            var response = _helper.GetData("/api/Challenge/GetNext?email=" + userEmail);

            if(response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                throw new Exception("Challenge retrieval failed");
            }
            return Ok(response.Content.ReadAsStringAsync().Result);
        }

        [HttpPost]
        public IActionResult PostChallenge([FromBody]ChallengeDto challengedto)
        {
            var response = _helper.Post("/api/Challenge/PostChallenge", challengedto);

            return Ok(response.Content.ReadAsStringAsync().Result);
        }

        [HttpGet]
        public IActionResult HasActiveTestForCandidate()
        {
            var userEmail = this.User.Identities.First().Name;

            var response = _helper.GetData("/api/Test/HasActiveTestForCandidate?email=" + userEmail);

            return Ok(response.Content.ReadAsStringAsync().Result);
        }

        [HttpGet]
        public IActionResult GetAllFinishedTests()
        {
            var response = _helper.GetData("/api/Test/GetFinishedTests");

            return Ok(response.Content.ReadAsStringAsync().Result);
        }

        public IActionResult GetTestById([FromUri]long id)
        {
            var response = _helper.GetData("/api/Test/GetTestById?id=" + id);
            return Ok(response.Content.ReadAsStringAsync().Result);
        }
    }
}
