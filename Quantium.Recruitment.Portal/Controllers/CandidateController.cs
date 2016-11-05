﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using IdentityModel.Client;
using System.Net.Http;
using System.Net.Http.Headers;
using Quantium.Recruitment.Portal.Helpers;
using Quantium.Recruitment.ApiServiceModels;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Identity;
using Quantium.Recruitment.Portal.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Quantium.Recruitment.Portal.Controllers
{
    [Authorize]
    public class CandidateController : Controller
    {
        private readonly IHttpHelper _helper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public CandidateController(
            IHttpHelper helper,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _helper = helper;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        // GET: /<controller>/
        public IActionResult Test()
        {
            return View();
        }
        
        public IActionResult Add()
        {
            var file = Request.Form.Files[0];

            string[] contentAsLines = GetContentFromFile(file);

            string[] headers = contentAsLines[0].Split(',');
            //var fileName = file.FileName;
            IList<CandidateDto> candidates = new List<CandidateDto>();

            for (int i = 1; i < contentAsLines.Length; i++)
            {
                candidates.Add(ParseLineToCandidate(headers, contentAsLines[i]));
            }

            try
            {
                var response = _helper.Post("api/Candidate/AddCandidates", candidates);
            }
            catch (Exception ex)
            {
                return Json(ex);
            }

            return Created(string.Empty, string.Empty);
        }

        private string[] GetContentFromFile(IFormFile file)
        {

            string fileContent;

            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                fileContent = reader.ReadToEnd();
            }

            return fileContent.Split(new string[] { "\r\n" }, StringSplitOptions.None).Where(item => item.Trim().Length > 0).ToArray();
        }

        private CandidateDto ParseLineToCandidate(string[] headers, string candidateLine)
        {
            string[] candidateOptions = candidateLine.Split(',');            

            CandidateDto newCandidate = new CandidateDto
            {
                FirstName = candidateOptions[1],
                LastName = candidateOptions[2],
                Email = candidateOptions[3]
            };

            return newCandidate;
        }

        public IActionResult GetAllCandidates()
        {
            var response = _helper.GetData("/api/Candidate/GetAllCandidates");

            return Ok(response.Content.ReadAsStringAsync().Result);
        }

        public IActionResult GetCandidateName()
        {
            var response = _helper.GetData("/api/Candidate/GetCandidateName?email=" + this.User.Identities.First().Name);

            return Ok(response.Content.ReadAsStringAsync().Result);

        }
        public IActionResult GetRoleName()
        {
            return Json("");
            //return Json(this.User.Claims.SingleOrDefault(claim => claim.Type.Contains("role")).Value);
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
        public IActionResult SaveCandidate([FromBody]List<CandidateDto> candidateDtos)
        {
            var response = _helper.Post("/api/Candidate/AddCandidates", candidateDtos);

            return Ok(response.Content.ReadAsStringAsync().Result);
        }

        [HttpGet]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(AccountController.Login), "Account");
        }

    }
}
