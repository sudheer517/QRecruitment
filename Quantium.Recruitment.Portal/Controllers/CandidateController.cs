using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using IdentityModel.Client;
using Simple.OData.Client;
using System.Net.Http;
using System.Net.Http.Headers;
using ODataModels.Quantium.Recruitment.ApiServices.Models;
using Quantium.Recruitment.Portal.Helpers;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Quantium.Recruitment.Portal.Controllers
{
    //[Authorize]
    public class CandidateController : Controller
    {
        private readonly ICandidateHelper _candidateHelper;

        public CandidateController(ICandidateHelper candidateHelper)
        {
            _candidateHelper = candidateHelper;
        }
        // GET: /<controller>/
        public IActionResult Test()
        {
            return View();
        }
        
        public IActionResult GetRoleName()
        {
            return Json("");
            //return Json(this.User.Claims.SingleOrDefault(claim => claim.Type.Contains("role")).Value);
        }
    }
}
