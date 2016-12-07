using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Quantium.Recruitment.Portal.Helpers;
using Quantium.Recruitment.Models;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Quantium.Recruitment.Portal.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
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
        public HttpResponseMessage AddAdmin([FromBody] AdminDto adminDto)
        {
            var response = _httpHelper.Post("/api/Admin/AddAdmin", adminDto);

            if (response.StatusCode != HttpStatusCode.Created)
                return new HttpResponseMessage(HttpStatusCode.NotFound);

            return response;
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
