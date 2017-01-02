using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using IdentityModel.Client;
using System.Net.Http;
using System.Net.Http.Headers;
using Quantium.Recruitment.Portal.Helpers;
using Quantium.Recruitment.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Identity;
using Quantium.Recruitment.Portal.Models;
using System.Net;
using AutoMapper;
using Quantium.Recruitment.Entities;
using Excel;
using System.Data;
using ClosedXML.Excel;
using System.ComponentModel.DataAnnotations;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Quantium.Recruitment.Portal.Controllers
{
    [Authorize]
    public class CandidateController : Controller
    {
        private readonly IHttpHelper _helper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<QRecruitmentRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ICandidateHelper _candidateHelper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CandidateController(
            IHttpHelper helper,
            UserManager<ApplicationUser> userManager,
            RoleManager<QRecruitmentRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            ICandidateHelper candidateHelper,
            IHttpContextAccessor httpContextAccessor)
        {
            _helper = helper;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _candidateHelper = candidateHelper;
            _httpContextAccessor = httpContextAccessor;
        }
        // GET: /<controller>/
        public IActionResult Test()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add()
        {
            var file = Request.Form.Files[0];
            //_httpContextAccessor.HttpContext.Request
            MemoryStream m = new MemoryStream();
            Stream strm=file.OpenReadStream();
            await strm.CopyToAsync(m);
            var candidateDtos = ParseInputCandidateFile(m);           
            var response = _helper.Post("api/Candidate/AddCandidates", file.OpenReadStream());
           
            var candidates = Mapper.Map<List<Candidate>>(candidateDtos);

            var responseStream = response.Content.ReadAsStreamAsync().Result;
            StreamReader reader = new StreamReader(responseStream);
            var result = reader.ReadToEnd();

            if (response.StatusCode != HttpStatusCode.Created)
            {
                return BadRequest(result);
            }
            await RegisterCandidate(candidates);

            return Created(string.Empty, string.Empty);
        }

        [HttpPost]
        public IActionResult PreviewCandidates()
        {
            var file = Request.Form.Files[0];

            HttpResponseMessage response = _helper.Post("api/Candidate/PreviewCandidates", file.OpenReadStream());

            var responseStream = response.Content.ReadAsStreamAsync().Result;
            StreamReader reader = new StreamReader(responseStream);
            var result = reader.ReadToEnd();

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        public IActionResult GetAllCandidates()
        {
            var response = _helper.GetData("/api/Candidate/GetAllCandidates");

            return Ok(response.Content.ReadAsStringAsync().Result);
        }

        public IActionResult GetCandidatesWithoutActiveTests()
        {
            var response = _helper.GetData("/api/Candidate/GetCandidatesWithoutActiveTests");

            return Ok(response.Content.ReadAsStringAsync().Result);
        }

        public IActionResult GetCandidateName()
        {
            var response = _helper.GetData("/api/Candidate/GetCandidateName?email=" + this.User.Identities.First().Name);

            return Ok(response.Content.ReadAsStringAsync().Result);

        }

        public IActionResult GetRoleName()
        {
            return Json(this.User.Claims.SingleOrDefault(claim => claim.Type.Contains("role")).Value);
        }

        public IActionResult IsInformationFilled()
        {
            var userEmail = this.User.Identities.First().Name;

            var response = _helper.GetData("/api/Candidate/IsInformationFilled?email=" + userEmail);

            return Ok(response.Content.ReadAsStringAsync().Result);
        }

        [HttpPost]
        public IActionResult FillCandidateInformation([FromBody]CandidateDto candidateDto)
        {
            var userEmail = this.User.Identities.First().Name;
            candidateDto.Email = userEmail;
            candidateDto.IsActive = true;
            candidateDto.IsInformationFilled = true;
            var response = _helper.Post("/api/Candidate/FillCandidateInformation", candidateDto);

            return Ok(response.Content.ReadAsStringAsync().Result);
        }

        [HttpPost]
        public async Task<IActionResult> SaveCandidate([FromBody]List<CandidateDto> candidateDtos)
        {
            var response = _helper.Post("/api/Candidate/Add", candidateDtos);

            if(response.StatusCode != HttpStatusCode.Created)
            {
                throw new Exception("Candidate creation failed");
            }
            var candidates = Mapper.Map<List<Candidate>>(candidateDtos);
            await RegisterCandidate(candidates);

            return Ok(response.Content.ReadAsStringAsync().Result);
        }
        private async Task RegisterCandidate(List<Candidate> candidates)
        {
            foreach (var candidate in candidates)
            {
                var userRole = _candidateHelper.GetRoleForEmail(candidate.Email);
                var user = new ApplicationUser { UserName = candidate.Email, Email = candidate.Email };
                
                string password=_candidateHelper.GeneratePassword();
                var result = await _userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    var _emailSender = new MessageSender();
                    await _emailSender.SendEmailAsync(candidate.Email, "Credentials for Login", string.Format("Please use below credentials for Login \\n {0} \\n {1}",
                        candidate.Email, password));
                    IdentityResult roleCreationResult = null;

                    if (!_roleManager.RoleExistsAsync(userRole).Result)
                    {
                        roleCreationResult = _roleManager.CreateAsync(new QRecruitmentRole(userRole)).Result;
                    }

                    var addUserToRoleTaskResult = _userManager.AddToRoleAsync(user, userRole).Result;
                }
            }
            return ;

        }
        private List<CandidateDto> ParseInputCandidateFile(MemoryStream ms)
        {
            //var httpRequest = _httpContextAccessor.HttpContext.Request;

            List<CandidateDto> candidateDtos = new List<CandidateDto>();
            using (ms)
            {
                //httpRequest.Body.CopyToAsync(ms);
                
                IExcelDataReader reader = ExcelReaderFactory.CreateOpenXmlReader(ms);
                DataSet dataset = reader.AsDataSet();
                var count = 1;
                List<string> headers = new List<string>();
                foreach (DataRow item in dataset.Tables[0].Rows)
                {
                    if (count == 1)
                    {
                        item.ItemArray.ForEach(i => headers.Add(i.ToString()));
                    }
                    else
                    {
                        List<string> candidateColumns = new List<string>();
                        item.ItemArray.ForEach(i => candidateColumns.Add(i.ToString()));

                        var email = candidateColumns[3];

                        if (!IsValidEmail(email))
                        {
                            string message = "Email " + email + " is not in correct format";
                            throw new ApplicationException(message);
                        }

                        CandidateDto newCandidate = new CandidateDto
                        {
                            Id = Convert.ToInt32(candidateColumns[0]),
                            FirstName = candidateColumns[1],
                            LastName = candidateColumns[2],
                            Email = email
                        };

                        candidateDtos.Add(newCandidate);
                    }
                    count++;
                }
            }

            return candidateDtos;
        }

        private bool IsValidEmail(string input)
        {
            return new EmailAddressAttribute().IsValid(input);
        }

        [HttpGet]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(AccountController.Login), "Account");
        }

    }
}
