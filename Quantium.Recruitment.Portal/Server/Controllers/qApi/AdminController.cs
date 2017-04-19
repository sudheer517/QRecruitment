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
using Microsoft.AspNetCore.Hosting;

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
        private readonly IHostingEnvironment _env;

        public AdminController(IEntityBaseRepository<Admin> adminRepository, 
            IHttpContextAccessor httpContextAccessor,
             UserManager<QRecruitmentUser> userManager,
            RoleManager<QRecruitmentRole> roleManager,
            IAccountHelper accountHelper,
            IEmailSender emailSender,
            IHostingEnvironment env)
        {
            _adminRepository = adminRepository;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _roleManager = roleManager;
            _accountHelper = accountHelper;
            _emailSender = emailSender;
            _env = env;
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
                    admin.IsActive = true;
                    _adminRepository.Add(admin);
                    await CreateUserWithAdminRole(admin);
                    return Created("created", admin);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message + ex.InnerException.Message + "unable to add admin");
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

            UserCreationModel userModel = new UserCreationModel { Username = admin.Email };

            userModel.Username = admin.Email;

            if (!socialLogins.Any(emailType => admin.Email.Contains(emailType)))
            {
                var user = new QRecruitmentUser { UserName = admin.Email, Email = admin.Email, IsEnabled = true, CreatedDate = DateTime.UtcNow };
                var password = AccountHelper.GenerateRandomString();
                userModel.Password = password;

                var result = await _userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    var addUserToRoleTaskResult = _userManager.AddToRoleAsync(user, Roles.Admin).Result;
                }

                await SendEmails(userModel, string.IsNullOrEmpty(admin.FirstName) ? admin.Email.Split('@').FirstOrDefault() : admin.FirstName);
            }
            else
            {
                await SendEmailWithoutPassword(userModel, string.IsNullOrEmpty(admin.FirstName) ? admin.Email.Split('@').FirstOrDefault() : admin.FirstName);
            }
            

            return;

        }

        private async Task<bool> SendEmails(UserCreationModel userModel, string firstName)
        {
            var Content = System.Net.WebUtility.HtmlDecode(System.IO.File.ReadAllText(System.IO.Path.Combine(_env.WebRootPath, "templates\\AdminCreationEmailTemplate.html")));
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("Username", userModel.Username);
            parameters.Add("Password", userModel.Password);
            parameters.Add("Admin", firstName);
            foreach (var param in parameters)
            {

                Content = Content.Replace("<" + param.Key + ">", param.Value);
            }

            var emailTask = _emailSender.SendEmailAsync(new EmailModel
            {
                To = userModel.Username,
                From = Startup.Configuration["RecruitmentAdminEmail"],
                DisplayName = "Quantium Recruitment",
                Subject = "User credentials",
                HtmlBody = Content
            });

            await Task.Run(() => emailTask);

            return true;
        }

        private async Task<bool> SendEmailWithoutPassword(UserCreationModel userModel, string firstName)
        {
            var Content = System.Net.WebUtility.HtmlDecode(System.IO.File.ReadAllText(System.IO.Path.Combine(_env.WebRootPath, "templates\\SocialAdminCreationEmailTemplate.html")));
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("Admin", firstName);
            foreach (var param in parameters)
            {
                Content = Content.Replace("<" + param.Key + ">", param.Value);
            }

            var emailTask = _emailSender.SendEmailAsync(new EmailModel
            {
                To = userModel.Username,
                From = Startup.Configuration["RecruitmentAdminEmail"],
                DisplayName = "Quantium Recruitment",
                Subject = "User credentials",
                HtmlBody = Content
            });

            await Task.Run(() => emailTask);

            return true;
        }
    }
}