using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Quantium.Recruitment.Portal.Helpers;
using Quantium.Recruitment.Models;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Quantium.Recruitment.Portal.Controllers
{
    [Authorize]
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
        [Authorize(Roles = "SuperAdmin")]
        public IActionResult GetAllFinishedTests()
        {
            var response = _helper.GetData("/api/Test/GetFinishedTests");

            return Ok(response.Content.ReadAsStringAsync().Result);
        }

        [Authorize(Roles = "SuperAdmin")]
        public IActionResult GetTestById(long id)
        {
            var response = _helper.GetData("/api/Test/GetTestById?id=" + id);
            return Ok(response.Content.ReadAsStringAsync().Result);
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public IActionResult GetFinishedTestById(long id)
        {
            var response = _helper.GetData("/api/Test/GetFinishedTestById?id=" + id);
            return Ok(response.Content.ReadAsStringAsync().Result);
        }

        [HttpPost]
        public HttpResponseMessage FinishTest([FromBody]long id)
        {
            var response = _helper.Post("/api/Test/FinishTest", id);
            if (response.StatusCode != HttpStatusCode.Accepted)
                throw new Exception("Finish test failed");

            return new HttpResponseMessage(HttpStatusCode.Accepted);
        }
    }
}
