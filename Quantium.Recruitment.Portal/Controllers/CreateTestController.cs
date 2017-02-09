using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Quantium.Recruitment.Models;
using Quantium.Recruitment.Portal.Helpers;
using Quantium.Recruitment.Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using AutoMapper;
using Newtonsoft.Json;

namespace Quantium.Recruitment.Portal.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class CreateTestController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<QRecruitmentRole> _roleManager;
        private readonly IHttpHelper _helper;      

        public CreateTestController(UserManager<ApplicationUser> userManager, RoleManager<QRecruitmentRole> roleManager, IHttpHelper helper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _helper = helper;
         
        }

        [HttpPost]
        public async Task<HttpResponseMessage> GenerateTests([FromBody] List<Candidate_JobDto> candidateJobDtos)
        {
            var response = _helper.Post("/api/Test/GenerateTests", candidateJobDtos);
            if (response.StatusCode != HttpStatusCode.Created)
                throw new Exception("Test creation failed");
            var _emailSender = new MessageSender();
            var candidateReponse = _helper.Post("/api/Candidate/GetCandidatesForTest",candidateJobDtos);
            List<CandidateDto> candidatesForTest = JsonConvert.DeserializeObject<List<CandidateDto>>(candidateReponse.Content.ReadAsStringAsync().Result);
            foreach (CandidateDto candidate in candidatesForTest)
            {               
                await _emailSender.SendEmailAsync(candidate.Email, "Test Created", string.Format("Hi {0}, \\n \\n A test was generated for you.Please login to our portal and complete the Test.",
                    candidate.FirstName));
            }
            return new HttpResponseMessage(HttpStatusCode.Created);
        }

        [HttpPost]
        public IActionResult SendTests([FromBody] List<CandidateDto> candidateDtos)
        {
            var response = _helper.Post("/api/Test/GenerateTests", candidateDtos);

            return Ok(response.Content.ReadAsStringAsync().Result);
        }
    }
}
