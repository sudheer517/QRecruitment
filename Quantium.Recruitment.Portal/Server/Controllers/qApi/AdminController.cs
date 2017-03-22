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
using AspNetCoreSpa.Server.Services.Abstract;
using AspNetCoreSpa;

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
        private readonly IEmailSender _emailSender;

        public AdminController(IEntityBaseRepository<Admin> adminRepository, 
            IHttpContextAccessor httpContextAccessor,
             UserManager<QRecruitmentUser> userManager,
            RoleManager<QRecruitmentRole> roleManager,
            IAccountHelper accountHelper,
            IEmailSender emailSender
            )
        {
            _adminRepository = adminRepository;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _roleManager = roleManager;
            _accountHelper = accountHelper;
            _emailSender = emailSender;
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
            var existingAdmin = _adminRepository.GetSingle(a => a.Email == adminDto.Email);
            if (existingAdmin == null)
            {
                var admin = Mapper.Map<Admin>(adminDto);

                try
                {
                    _adminRepository.Add(admin);
                    await CreateUserWithAdminRole(admin);
                    return Created("created", admin);
                }
                catch (Exception ex)
                {
                    return BadRequest("unable to add admin");
                }
            }
            else
            {
                return StatusCode(StatusCodes.Status409Conflict);
            }
        }

        private async Task CreateUserWithAdminRole(Admin admin)
        {
            var socialLogins = new List<string>()
            {
                "@outlook", "@live", "@hotmail", "@gmail", "@google"
            };

            UserCreationModel userModel = null;


            if (!socialLogins.Any(emailType => admin.Email.Contains(emailType)))
            {
                var user = new QRecruitmentUser { UserName = admin.Email, Email = admin.Email };
                var password = AccountHelper.GenerateRandomString();
                userModel = new UserCreationModel { Username = admin.Email, Password = password };

                var result = await _userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    var addUserToRoleTaskResult = _userManager.AddToRoleAsync(user, Roles.Admin).Result;
                }
            }

            await SendEmails(userModel, admin.FirstName);

            return;

        }

        private async Task<bool> SendEmails(UserCreationModel userModel, string firstName)
        {
            var emailTemplate = System.IO.File.ReadAllText(@"Server/Templates/AdminCreationEmailTemplate.html");

            var emailTask = _emailSender.SendEmailAsync(new EmailModel
            {
                To = userModel.Username,
                From = Startup.Configuration["RecruitmentAdminEmail"],
                DisplayName = "Quantium Recruitment",
                Subject = "User credentials",
                HtmlBody = string.Format(emailTemplate, firstName, userModel.Username, userModel.Password)
            });

            await Task.Run(() => emailTask);

            return true;
        }
    }
}