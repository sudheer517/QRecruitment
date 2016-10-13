using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Quantium.Recruitment.Portal.Helpers;
using Simple.OData.Client;
using Quantium.Recruitment.ApiServiceModels;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Quantium.Recruitment.Portal.Controllers
{
    public class AdminController : Controller
    {
        private ODataClient _odataClient;

        public AdminController(IOdataHelper _odataHelper)
        {
            _odataClient = _odataHelper.GetOdataClient();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetAdminList()
        {
            return Json(_odataClient.For<AdminDto>().FindEntriesAsync().Result);
        }

        [HttpPost]
        public IActionResult AddAdmin([FromBody] AdminDto adminDto)
        {
            // the htttp request content-type should be set to application/json
            return Json(_odataClient.For<AdminDto>().Set(adminDto).InsertEntryAsync());
        }

        [HttpPost]
        public IActionResult UpdateAdmin([FromBody] AdminDto adminDto)
        {
            // the htttp request content-type should be set to application/json
            return Json(_odataClient.For<AdminDto>().Key(adminDto.Id).Set(adminDto).UpdateEntriesAsync());
        }

        [HttpPost]
        public IActionResult DeleteAdmin([FromBody]int key)
        {
            return Json(_odataClient.For<AdminDto>().Key(key).DeleteEntryAsync());
        }

        public IActionResult GetDepartmentList()
        {
            return Json(_odataClient.For<DepartmentDto>().FindEntriesAsync().Result);
        }
    }
}
