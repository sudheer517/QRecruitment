using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Quantium.Recruitment.ApiServiceModels;
using Quantium.Recruitment.Portal.Helpers;
using Quantium.Recruitment.Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quantium.Recruitment.Portal.Controllers
{
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
        public IActionResult GenerateTests([FromBody] List<Candidate_JobDto> candidateJobDtos) 
        {
            var response = _helper.Post("api/Test/GenerateTests", candidateJobDtos);

            return Ok(response.Content.ReadAsStringAsync().Result);
        }
    }
}
