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
using AutoMapper;
using Quantium.Recruitment.Portal.Models;
using Microsoft.AspNetCore.Identity;
using Quantium.Recruitment.Entities;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Quantium.Recruitment.Portal.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class AdminController : Controller
    {
        private IHttpHelper _httpHelper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<QRecruitmentRole> _roleManager;
        private readonly ICandidateHelper _candidateHelper;

        public AdminController(IHttpHelper httpHelper,
            UserManager<ApplicationUser> userManager,
            RoleManager<QRecruitmentRole> roleManager,            
            ICandidateHelper candidateHelper)
        {
            _httpHelper = httpHelper;
            _userManager = userManager;
            _roleManager = roleManager;           
            _candidateHelper = candidateHelper;
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
        public async Task<HttpResponseMessage> AddAdmin([FromBody] AdminDto adminDto)
        {
            var response = _httpHelper.Post("/api/Admin/AddAdmin", adminDto);

            if (response.StatusCode != HttpStatusCode.Created)
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            var admin = Mapper.Map<Admin>(adminDto);
            await RegisterAdmin(admin);
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

        private async Task RegisterAdmin(Admin admin)
        {
              var userRole = _candidateHelper.GetRoleForEmail(admin.Email);
                var user = new ApplicationUser { UserName = admin.Email, Email = admin.Email };
                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {                   
                    IdentityResult roleCreationResult = null;

                    if (!_roleManager.RoleExistsAsync(userRole).Result)
                    {
                        roleCreationResult = _roleManager.CreateAsync(new QRecruitmentRole(userRole)).Result;
                    }

                    var addUserToRoleTaskResult = _userManager.AddToRoleAsync(user, userRole).Result;
                }
            
            return;

        }
    }
}
