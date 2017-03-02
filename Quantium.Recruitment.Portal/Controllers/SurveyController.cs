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

namespace Quantium.Recruitment.Portal.Controllers
{
    [Authorize]
    public class SurveyController : Controller
    {
        private readonly IHttpHelper _helper;

        public SurveyController(IHttpHelper helper)
        {
            _helper = helper;
        }

        [HttpGet]
        public IActionResult GetNextSurveyChallenge()
        {
            var userEmail = this.User.Identities.First().Name;

            var response = _helper.GetData("/api/SurveyChallenge/GetNext?email=" + userEmail);

            if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                throw new Exception("Survey Challenge retrieval failed");
            }
            return Ok(response.Content.ReadAsStringAsync().Result);
        }

        [HttpPost]
        public IActionResult PostSurveyChallenge([FromBody]SurveyChallengeDto surveychallengedto)
        {            
            var response = _helper.Post("/api/SurveyChallenge/PostSurveyChallenge", surveychallengedto);

            return Ok(response.Content.ReadAsStringAsync().Result);
        }

        [HttpPost]
        public IActionResult ArchiveSurveys([FromBody]long[] surveyIds)
        {
            var response = _helper.Post("/api/Test/ArchiveSurveys", surveyIds);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new Exception("Unable to archive survey");

            return Ok(response.Content.ReadAsStringAsync().Result);
        }


        [HttpGet]
        public IActionResult HasActiveSurveyForCandidate()
        {
            var userEmail = this.User.Identities.First().Name;

            var response = _helper.GetData("/api/Survey/HasActiveSurveyForCandidate?email=" + userEmail);

            return Ok(response.Content.ReadAsStringAsync().Result);
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public IActionResult GetAllFinishedSurveys()
        {
            var response = _helper.GetData("/api/Survey/GetFinishedSurveys");

            return Ok(response.Content.ReadAsStringAsync().Result);
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public IActionResult GetAllSurveys()
        {
            var response = _helper.GetData("/api/Survey/GetAllSurveys");

            return Ok(response.Content.ReadAsStringAsync().Result);
        }

        [Authorize(Roles = "SuperAdmin")]
        public IActionResult GetSurveyById(long id)
        {
            var response = _helper.GetData("/api/Test/GetSurveyById?id=" + id);
            return Ok(response.Content.ReadAsStringAsync().Result);
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public IActionResult GetFinishedSurveyById(long id)
        {
            var response = _helper.GetData("/api/Test/GetFinishedSurveyById?id=" + id);
            return Ok(response.Content.ReadAsStringAsync().Result);
        }

        [HttpPost]
        public HttpResponseMessage FinishSurvey([FromBody]long id)
        {
            var response = _helper.Post("/api/Test/FinishSurvey", id);
            if (response.StatusCode != HttpStatusCode.Accepted)
                throw new Exception("Finish test failed");

            return new HttpResponseMessage(HttpStatusCode.Accepted);
        }
    }
}
