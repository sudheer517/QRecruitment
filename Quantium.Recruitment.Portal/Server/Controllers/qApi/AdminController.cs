using Quantium.Recruitment.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using AutoMapper;
using Quantium.Recruitment.Models;
using Quantium.Recruitment.Entities;
using Quantium.Recruitment.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using AspNetCoreSpa.Server.Repositories.Abstract;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using AspNetCoreSpa.Server.Entities;
using Quantium.Recruitment.Portal.Server.Entities;
using Quantium.Recruitment.Portal.Server.Helpers;

namespace Quantium.Recruitment.ApiServices.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("[controller]/[action]/{id?}")]
    public class AdminController : Controller
    {
        private readonly IEntityBaseRepository<Admin> _adminRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<QRecruitmentUser> _userManager;
        private readonly RoleManager<QRecruitmentRole> _roleManager;
        private readonly IAccountHelper _accountHelper;

        public AdminController(IEntityBaseRepository<Admin> adminRepository, 
            IHttpContextAccessor httpContextAccessor,
             UserManager<QRecruitmentUser> userManager,
            RoleManager<QRecruitmentRole> roleManager,
            IAccountHelper accountHelper
            )
        {
            _adminRepository = adminRepository;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _roleManager = roleManager;
            _accountHelper = accountHelper;
        }

        [HttpGet]
        public IActionResult IsAdmin(string email)
        {
            var admin = _adminRepository.GetAll().SingleOrDefault(a => a.Email == email && a.IsActive == true);

            return Ok(Mapper.Map<AdminDto>(admin));
        }

        [HttpGet]
        public IActionResult GetAdmin(int key)
        {
            var admin = _adminRepository.GetSingle(item => item.Id == key);

            return Ok(Mapper.Map<AdminDto>(admin));
        }

        [HttpGet]
        public IActionResult GetAdminByEmail(string email)
        {
            var admin = _adminRepository.FindBy(ad => ad.Email == email).FirstOrDefault();

            if (admin != null)
                return Ok(Mapper.Map<AdminDto>(admin));
            else
                return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> AddAdminAsync([FromBody]AdminDto adminDto)
        {
            var admin = Mapper.Map<Admin>(adminDto);

            try
            {
                _adminRepository.Add(admin);
                await RegisterAdmin(admin);
                return Created("created", admin);
            }
            catch (Exception ex)
            {
                return BadRequest("unable to add admin");
            }
        }

        private async Task RegisterAdmin(Admin admin)
        {
            var userRole = _accountHelper.GetRoleForEmail(admin.Email);
            var user = new QRecruitmentUser { UserName = admin.Email, Email = admin.Email };
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