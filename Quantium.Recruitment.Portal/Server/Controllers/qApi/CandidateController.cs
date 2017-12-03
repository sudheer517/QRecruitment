using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Quantium.Recruitment.Models;
using Quantium.Recruitment.Entities;
using AspNetCoreSpa.Server.Repositories.Abstract;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using Quantium.Recruitment.Portal.Server.Helpers;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using AspNetCoreSpa.Server.Entities;
using Quantium.Recruitment.Portal.Server.Entities;
using AspNetCoreSpa.Server.Services.Abstract;
using AspNetCoreSpa;
using Microsoft.AspNetCore.Hosting;

namespace Quantium.Recruitment.Portal.Server.Controllers.qApi
{
    [Authorize(Roles = "Admin, Candidate")]
    [Route("[controller]/[action]/{id?}")]
    public class CandidateController : Controller
    {
        private readonly IEntityBaseRepository<Candidate> _candidateRepository;
        private readonly IEntityBaseRepository<Admin> _adminRepository;
        private readonly UserManager<QRecruitmentUser> _userManager;
        private readonly RoleManager<QRecruitmentRole> _roleManager;
        private readonly IAccountHelper _accountHelper;
        private readonly IEmailSender _emailSender;
        private readonly IHostingEnvironment _env;

        public CandidateController(IEntityBaseRepository<Candidate> candidateRepository,
            IEntityBaseRepository<Admin> adminRepository,
            UserManager<QRecruitmentUser> userManager,
            RoleManager<QRecruitmentRole> roleManager,
            IAccountHelper accountHelper,
            IEmailSender emailSender,
            IHostingEnvironment env)
        {
            _candidateRepository = candidateRepository;
            _userManager = userManager;
            _roleManager = roleManager;
            _accountHelper = accountHelper;
            _emailSender = emailSender;
            _adminRepository = adminRepository;
            _env = env;
        }

        //Accessible only by admin

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddCandidateAsync([FromBody]CandidateDto candidateDto)
        {
            var adminEmail = this.User.Identities.First().Name;

            var admin = _adminRepository.GetSingle(a => a.Email == adminEmail);

            var existingCandidate = _candidateRepository.FindBy(c => c.Email == candidateDto.Email).FirstOrDefault();

            if(existingCandidate == null)
            {
                var candidate = Mapper.Map<Candidate>(candidateDto);
                candidate.IsActive = true;
                candidate.CreatedUtc = DateTime.UtcNow;
                candidate.AdminId = admin.Id;
                IList<UserCreationModel> userModels = new List<UserCreationModel>();
                try
                {
                    _candidateRepository.Add(candidate);

                    var socialLogins = new List<string>()
                        {
                            "@outlook", "@live", "@hotmail", "@gmail", "@google"
                        };

                    if (!socialLogins.Any(emailType => candidate.Email.Contains(emailType)))
                    {
                        var user = new QRecruitmentUser { UserName = candidate.Email, Email = candidate.Email, CreatedDate = DateTime.UtcNow };
                        var password = AccountHelper.GenerateRandomString();
                        user.PlainPassword = password;
                        userModels.Add(new UserCreationModel { Username = candidate.Email, Password = password });

                        var result = await _userManager.CreateAsync(user, password);

                        if (result.Succeeded)
                        {
                            var addedToRoleResult = await _userManager.AddToRoleAsync(user, Roles.Candidate);
                        }
                        else
                        {
                            return StatusCode(StatusCodes.Status409Conflict, candidate);
                        }
                    }

                    await SendEmails(userModels);

                    return Created("created", candidate);
                }
                catch (Exception ex)
                {
                    return BadRequest("unable to add candidate" + ex.Message);
                }
            }
            else
            {
                return StatusCode(StatusCodes.Status409Conflict, JsonConvert.SerializeObject("Duplicate"));
            }
            
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult PreviewCandidates()
        {
            try
            {
                IList<CandidateDto> candidateDtos;
                var statusCode = GetCandidateDtosFromWorkSheet(out candidateDtos);

                if (statusCode == StatusCodes.Status200OK)
                {
                    foreach (var candidateDto in candidateDtos)
                    {
                        var candidate = _candidateRepository.GetSingle(c => c.Email == candidateDto.Email);

                        if (candidate != null)
                        {
                            return StatusCode(StatusCodes.Status409Conflict, candidate);
                        }
                    }

                    return Ok(candidateDtos);
                }
                else
                {
                    return StatusCode(statusCode, JsonConvert.SerializeObject("Validation failed"));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status406NotAcceptable, ex.Message);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var candidates = await _candidateRepository.FindByAsync(c => c.IsActive == true);

            //set firstname and lastname to empty rather than null because of filtering bug on client side
            var cDtos = Mapper.Map<IList<CandidateDto>>(candidates,
                opts => opts.AfterMap((src, dest) => {
                    var candidateList = (List<CandidateDto>)dest;
                    candidateList.ForEach(candidateDto => {
                        candidateDto.FirstName = candidateDto.FirstName == null ? candidateDto.FirstName = "" : candidateDto.FirstName;
                        candidateDto.LastName = candidateDto.LastName == null ? candidateDto.LastName = "" : candidateDto.LastName;
                    });
                }));

            return Ok(cDtos);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllWithPassword()
        {
            var candidates = await _candidateRepository.FindByAsync(c => c.IsActive == true);
            var candidateRoleUsers = await _userManager.GetUsersInRoleAsync(Roles.Candidate);
            var candidateEmailToPasswordMap = candidateRoleUsers.ToDictionary(key => key.Email, value => value.PlainPassword);

            //set firstname and lastname to empty rather than null because of filtering bug on client side
            var cDtos = Mapper.Map<IList<CandidateDto>>(candidates,
                opts => opts.AfterMap((src, dest) => {
                    var candidateList = (List<CandidateDto>)dest;
                    candidateList.ForEach(candidateDto => {
                        candidateDto.FirstName = candidateDto.FirstName ?? string.Empty;
                        candidateDto.LastName = candidateDto.LastName ?? string.Empty;
                        var passwordValue = string.Empty;
                        candidateEmailToPasswordMap.TryGetValue(candidateDto.Email, out passwordValue);
                        candidateDto.Password = passwordValue ?? string.Empty;
                    });
                }));

            return Ok(cDtos);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult GetCandidatesWithoutActiveTests()
        {
            var candidates = _candidateRepository.AllIncluding(c => c.Tests).Where(c => c.IsActive && c.Tests.Count() == 0).OrderByDescending(c => c.CreatedUtc).ToList();

            return Ok(Mapper.Map<IList<CandidateDto>>(candidates));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddCandidatesAsync()
        {
            try
            {
                IList<CandidateDto> candidateDtos;
                var statusCode = GetCandidateDtosFromWorkSheet(out candidateDtos);

                if (statusCode == StatusCodes.Status200OK)
                {
                    var candidates = Mapper.Map<List<Candidate>>(candidateDtos);

                    var adminEmail = this.User.Identities.First().Name;
                    var admin = await _adminRepository.GetSingleAsync(a => a.Email == adminEmail);
                    var isDuplicatesFound = false;

                    IList<UserCreationModel> userModels = new List<UserCreationModel>();

                    foreach (var candidate in candidates)
                    {
                        var candidateEntity = await _candidateRepository.GetSingleAsync(c => c.Email == candidate.Email);

                        if (candidateEntity != null)
                        {
                            isDuplicatesFound = true;
                            //return StatusCode(StatusCodes.Status409Conflict, candidate);
                            continue;
                        }

                        candidate.CreatedUtc = DateTime.UtcNow;
                        candidate.AdminId = admin.Id;
                        candidate.IsActive = true;
                        _candidateRepository.Add(candidate);

                        var socialLogins = new List<string>()
                        {
                            "@outlook", "@live", "@hotmail", "@gmail", "@google"
                        };

                        if (!socialLogins.Any(emailType => candidate.Email.Contains(emailType)))
                        {
                            var user = new QRecruitmentUser { UserName = candidate.Email, Email = candidate.Email, CreatedDate = DateTime.UtcNow };
                            var password = AccountHelper.GenerateRandomString();
                            user.PlainPassword = password;
                            userModels.Add(new UserCreationModel { Username = candidate.Email, Password = password });

                            var result = await _userManager.CreateAsync(user, password);

                            if (result.Succeeded)
                            {
                                var addedToRoleResult = await _userManager.AddToRoleAsync(user, Roles.Candidate);
                            }
                            else
                            {
                                return StatusCode(StatusCodes.Status400BadRequest, candidate);
                            }
                        }
                    }

                    await SendEmails(userModels);

                    if (isDuplicatesFound)
                    {
                        return StatusCode(StatusCodes.Status409Conflict);
                    }

                    return Created(string.Empty, JsonConvert.SerializeObject("created"));
                }
                else
                {
                    return StatusCode(statusCode);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private int GetCandidateDtosFromWorkSheet(out IList<CandidateDto> candidateDtos)
        {
            var file = Request.Form.Files[0];

            var fs = file.OpenReadStream();

            var excelPackage = new ExcelPackage(fs);
            fs.Dispose();

            var workSheet = excelPackage.Workbook.Worksheets.FirstOrDefault();

            var row = workSheet.Dimension.Start.Row;
            var end = workSheet.Dimension.End.Row;

            IList<string> headers = new List<string>();

            candidateDtos = new List<CandidateDto>();

            for (int rowIndex = workSheet.Dimension.Start.Row; rowIndex <= workSheet.Dimension.End.Row; rowIndex++)
            {
                if (rowIndex == 1)
                {
                    headers = ExcelHelper.GetExcelHeaders(workSheet, rowIndex);
                }
                else
                {
                    IList<string> candidateDetails = ExcelHelper.GetExcelHeaders(workSheet, rowIndex);

                    var email = candidateDetails[3];

                    if (!IsValidEmail(email))
                    {
                        string message = "Email " + email + " is not in correct format";
                        throw new Exception(message);
                    }

                    CandidateDto newCandidate = new CandidateDto
                    {
                        FirstName = candidateDetails[1].Trim(),
                        LastName = candidateDetails[2].Trim(),
                        Email = email.Trim()
                    };

                    candidateDtos.Add(newCandidate);
                }

            }

            if (candidateDtos.Count == 0)
            {
                return StatusCodes.Status422UnprocessableEntity;
            }

            var distinctCandidates = candidateDtos.DistinctBy(c => c.Email);

            if (distinctCandidates.Count() != candidateDtos.Count)
            {
                return StatusCodes.Status409Conflict;
            }

            return StatusCodes.Status200OK;
        }

        private bool IsValidEmail(string input)
        {
            return new EmailAddressAttribute().IsValid(input);
        }

        //Accessible by both candidate and admin

        [HttpGet]
        public IActionResult Get()
        {
            var email = this.User.Identities.First().Name;
            var candidateDto = Mapper.Map<CandidateDto>(_candidateRepository.GetSingle(c => c.Email == email));
            return Ok(candidateDto);
        }

        [HttpGet]
        public IActionResult IsInformationFilled()
        {
            var email = this.User.Identities.First().Name;
            var candidateDto = Mapper.Map<CandidateDto>(_candidateRepository.GetSingle(c => c.Email == email));
            return Ok(JsonConvert.SerializeObject(candidateDto.IsInformationFilled));
        }

        [HttpPost]
        public IActionResult SaveDetails([FromBody]CandidateDto candidateDto)
        {
            var email = this.User.Identities.First().Name;
            var candidate = _candidateRepository.GetSingle(c => c.Email == email);

            candidate.IsInformationFilled = true;
            candidate.FirstName = candidateDto.FirstName;
            candidate.LastName = candidateDto.LastName;
            candidate.Mobile = candidateDto.Mobile;
            candidate.City = candidateDto.City;
            candidate.Country = candidateDto.Country;
            candidate.College = candidateDto.College;
            candidate.Branch = candidateDto.Branch;
            candidate.CGPA = candidateDto.CGPA;
            candidate.CurrentCompany = candidateDto.CurrentCompany;
            candidate.ExperienceInYears = candidateDto.ExperienceInYears;
            candidate.PassingYear = candidateDto.PassingYear;
            candidate.State = candidateDto.State;

            _candidateRepository.Edit(candidate);
            _candidateRepository.Commit();
            return Ok(JsonConvert.SerializeObject("Saved"));
        }  
        

        private async Task<bool> SendEmails(IList<UserCreationModel> userModels)
        {
            var emailTemplate = System.Net.WebUtility.HtmlDecode(System.IO.File.ReadAllText(System.IO.Path.Combine(_env.WebRootPath, "templates\\UserCreationEmailTemplate.html")));
            foreach (var userModel in userModels)
            {
                var Content = emailTemplate;
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("Username", userModel.Username);
                parameters.Add("Password", userModel.Password);
                parameters.Add("candidateEmail", userModel.Username);

                foreach (var param in parameters)
                {

                    Content = Content.Replace("<" + param.Key + ">", param.Value);
                }

                await _emailSender.SendEmailAsync(new EmailModel
                {
                    To = userModel.Username,
                    From = Startup.Configuration["RecruitmentAdminEmail"],
                    DisplayName = "Quantium Recruitment",
                    Subject = "User credentials",
                    TextBody = Content
                });
            }
            
            return true;
        }

        [HttpPost]
        public async Task<IActionResult> ArchiveCandidates([FromBody]long[] testIds)
        {
            var candidates = await _candidateRepository.FindByAsync(t => testIds.Contains(t.Id));

            try
            {
                foreach (var candidate in candidates)
                {
                    candidate.IsActive = false;
                }

                _candidateRepository.Commit();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

            return Ok(JsonConvert.SerializeObject("Deleted"));
        }

        [HttpGet]
        public IActionResult Unsubscribe([FromQuery]string email)
        {
            var result = email;
            return View("You are unsubscribed");
        }
    }
}