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

namespace Quantium.Recruitment.Portal.Server.Controllers.qApi
{
    [Authorize(Roles = "Admin, Candidate")]
    [Route("[controller]/[action]/{id?}")]
    public class CandidateController : Controller
    {
        private readonly IEntityBaseRepository<Candidate> _candidateRepository;
        private readonly UserManager<QRecruitmentUser> _userManager;
        private readonly RoleManager<QRecruitmentRole> _roleManager;
        private readonly IAccountHelper _accountHelper;

        public CandidateController(IEntityBaseRepository<Candidate> candidateRepository,
            UserManager<QRecruitmentUser> userManager,
            RoleManager<QRecruitmentRole> roleManager,
            IAccountHelper accountHelper
            )
        {
            _candidateRepository = candidateRepository;
            _userManager = userManager;
            _roleManager = roleManager;
            _accountHelper = accountHelper;
        }

        //Accessible only by admin

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddCandidateAsync([FromBody]CandidateDto candidateDto)
        {
            var candidate = Mapper.Map<Candidate>(candidateDto);
            candidate.IsActive = true;
            candidate.CreatedUtc = DateTime.UtcNow;

            try
            {
                _candidateRepository.Add(candidate);
                List<Candidate> candidates = new List<Candidate>();
                candidates.Add(candidate);
                await RegisterCandidate(candidates);
                return Created("created", candidate);
            }
            catch (Exception ex)
            {
                return BadRequest("unable to add candidate");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult PreviewCandidates(ICollection<IFormFile> files)
        {
            var file = Request.Form.Files[0];

            var fs = file.OpenReadStream();

            var excelPackage = new ExcelPackage(fs);
            fs.Dispose();

            var workSheet = excelPackage.Workbook.Worksheets.FirstOrDefault();
            try
            {
                var candidateDtos = GetCandidateDtosFromWorkSheet(workSheet);
                return Ok(candidateDtos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAll()
        {
            var candidates = _candidateRepository.GetAll();

            var cDtos = Mapper.Map<IList<CandidateDto>>(candidates);

            return Ok(cDtos);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult GetCandidatesWithoutActiveTests()
        {
            var candidates = _candidateRepository.AllIncluding(c => c.Tests).Where(c => c.IsActive && c.Tests.Count() == 0).ToList();

            return Ok(Mapper.Map<IList<CandidateDto>>(candidates));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddCandidatesAsync(ICollection<IFormFile> files)
        {
            var file = Request.Form.Files[0];

            var fs = file.OpenReadStream();

            var excelPackage = new ExcelPackage(fs);
            fs.Dispose();

            var workSheet = excelPackage.Workbook.Worksheets.FirstOrDefault();
            try
            {
                var candidateDtos = GetCandidateDtosFromWorkSheet(workSheet);
                var candidates = Mapper.Map<List<Candidate>>(candidateDtos);
                foreach (var candidate in candidates)
                {
                    _candidateRepository.Add(candidate);
                }
                await RegisterCandidate(candidates);
                return Created(string.Empty, candidateDtos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        private IList<CandidateDto> GetCandidateDtosFromWorkSheet(ExcelWorksheet workSheet)
        {

            var row = workSheet.Dimension.Start.Row;
            var end = workSheet.Dimension.End.Row;

            IList<string> headers = new List<string>();

            IList<CandidateDto> candidateDtos = new List<CandidateDto>();

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
                        Id = Convert.ToInt32(candidateDetails[0]),
                        FirstName = candidateDetails[1],
                        LastName = candidateDetails[2],
                        Email = email
                    };


                    candidateDtos.Add(newCandidate);

                }

            }

            return candidateDtos;
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
        

        private async Task RegisterCandidate(List<Candidate> candidates)
        {
            foreach (var candidate in candidates)
            {
                var userRole = _accountHelper.GetRoleForEmail(candidate.Email);
                var user = new QRecruitmentUser { UserName = candidate.Email, Email = candidate.Email };
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
            }
            return;

        }
    }
}