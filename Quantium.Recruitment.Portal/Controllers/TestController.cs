using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Quantium.Recruitment.Portal.Helpers;
using Quantium.Recruitment.ApiServiceModels;

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
            return Json(_helper.GetData("/api/Department/GetAllDepartments"));
        }
    }
}
