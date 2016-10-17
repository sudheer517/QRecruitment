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
    public class AdminController : Controller
    {
        private IHttpHelper _httpHelper;

        public AdminController(IHttpHelper httpHelper)
        {
            _httpHelper = httpHelper;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetAdminList()
        {
            return Json("");
            //return Json(_odataClient.For<AdminDto>().FindEntriesAsync().Result);
        }

        [HttpPost]
        public IActionResult AddAdmin([FromBody] AdminDto adminDto)
        {
            return Json("");
            // the htttp request content-type should be set to application/json
            //return Json(_odataClient.For<AdminDto>().Set(adminDto).InsertEntryAsync());
        }

        [HttpPost]
        public IActionResult UpdateAdmin([FromBody] AdminDto adminDto)
        {
            return Json("");
            // the htttp request content-type should be set to application/json
            //return Json(_odataClient.For<AdminDto>().Key(adminDto.Id).Set(adminDto).UpdateEntriesAsync());
        }

        [HttpPost]
        public IActionResult DeleteAdmin([FromBody]int key)
        {
            return Json("");
            //return Json(_odataClient.For<AdminDto>().Key(key).DeleteEntryAsync());
        }

        public IActionResult GetDepartmentList()
        {
            return Json("");
            //return Json(_odataClient.For<DepartmentDto>().FindEntriesAsync().Result);
        }
    }
}
