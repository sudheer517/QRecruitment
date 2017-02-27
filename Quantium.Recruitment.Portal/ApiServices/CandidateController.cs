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
using System.Web;
using System.IO;
using Excel;
using System.Data;
using ClosedXML.Excel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace Quantium.Recruitment.ApiServices.Controllers
{
    //[Route("api/candidate")]
    [Route("api/[controller]/[action]/{id?}")]
    public class CandidateController : Controller
    {
        private readonly ICandidateRepository _candidateRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CandidateController(ICandidateRepository candidateRepository, IHttpContextAccessor httpContextAccessor)
        {
            _candidateRepository = candidateRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public IActionResult GetAllCandidates()
        {
            var candidates = _candidateRepository.GetAll().Where(candidate => candidate.IsActive).ToList();

            return Ok(Mapper.Map<IList<CandidateDto>>(candidates));
        }

        [HttpGet]
        public IActionResult GetCandidatesWithoutActiveTests()
        {
            var candidates = _candidateRepository.GetAll().Where(c => c.IsActive && c.Tests.Count() == 0).ToList();

            return Ok(Mapper.Map<IList<CandidateDto>>(candidates));
        }

        [HttpPost]
        public IActionResult UpdateCandidatesForTest([FromBody]List<Candidate_JobDto> candidateJob)
        {
            var candidateList = candidateJob.Select(x => x.Candidate.Id).ToList();
            var candidates = _candidateRepository.GetAll().Where(c => candidateList.Contains(c.Id));
            foreach (var candidate in candidates.ToList())
            {
                //TestMailSent Stages
                //Default =0
                //TestCreated=2
                //TestMailSent=3
                if (!(candidate.TestMailSent==2))
                {
                    candidate.TestMailSent = 2;
                    var updatedCandidate = (Candidate)Mapper.Map(candidate, candidate, typeof(Candidate), typeof(Candidate));
                    _candidateRepository.Update(updatedCandidate);
                }
            }
            return Ok();
        }

        [HttpGet]
        public IActionResult GetCandidateByEmail(string email)
        {
            var candidate = _candidateRepository.GetAll().SingleOrDefault(item => item.Email == email && item.IsActive == true);

            if (candidate != null)
                return Ok(Mapper.Map<CandidateDto>(candidate));
            else
                return Ok();
        }

        [HttpGet]
        public IActionResult IsInformationFilled(string email)
        {
            var candidate = _candidateRepository.GetAll().SingleOrDefault(item => item.Email == email && item.IsActive == true && item.IsInformationFilled == true);

            if (candidate != null)
                return Ok(true);
            else
                return Ok(false);
        }

        [HttpGet]
        public IActionResult GetSingleCandidate(int key)
        {
            var candidate = _candidateRepository.GetAll().Single(item => item.Id == key);

            return Ok(Mapper.Map<CandidateDto>(candidate));
        }

        [HttpGet]
        public IActionResult GetCandidateName(string email)
        {
            var candidate = _candidateRepository.GetAll().Single(item => item.Email == email);

            return Ok($"{candidate.FirstName} {candidate.LastName}");
        }

        private List<CandidateDto> ParseInputCandidateFile()
        {
            var httpRequest = _httpContextAccessor.HttpContext.Request;

            List<CandidateDto> candidateDtos = new List<CandidateDto>();
            using (var ms = new MemoryStream())
            {
                httpRequest.Body.CopyToAsync(ms);
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

        [HttpPost]
        public IActionResult Add([FromBody]List<CandidateDto> candidateDtos)
        {
            var candidates = Mapper.Map<List<Candidate>>(candidateDtos);

            foreach (var candidate in candidates)
            {
                bool isCandidateAlreadyAdded = _candidateRepository.FindByEmail(candidate.Email) == null ? false : true;
                if (!isCandidateAlreadyAdded)
                {
                    candidate.IsActive = true;
                    _candidateRepository.Add(candidate);
                }
            }

            return Created(string.Empty, "Candidates Created");
        }

        [HttpPost]
        public IActionResult AddCandidates()
        {
            var candidateDtos = ParseInputCandidateFile();

            var candidates = Mapper.Map<List<Candidate>>(candidateDtos);

            foreach (var candidate in candidates)
            {
                bool isCandidateAlreadyAdded = _candidateRepository.FindByEmail(candidate.Email) == null ? false : true;
                if (!isCandidateAlreadyAdded)
                {
                    candidate.IsActive = true;
                    _candidateRepository.Add(candidate);
                }
            }

            return Created(string.Empty, "Candidates Created");
        }

        [HttpPost]
        public IActionResult PreviewCandidates()
        {
            var candidateDtos = ParseInputCandidateFile();
            return Ok(candidateDtos);
        }

        [HttpPost]
        public IActionResult FillCandidateInformation([FromBody]CandidateDto candidateDto)
        {
            var candidate = _candidateRepository.FindByEmail(candidateDto.Email);

            var updatedCandidate = (Candidate)Mapper.Map(candidateDto, candidate, typeof(CandidateDto), typeof(Candidate));

            _candidateRepository.Update(updatedCandidate);

            return Ok("Candidate Created");
        }

        private bool IsValidEmail(string input)
        {
            return new EmailAddressAttribute().IsValid(input);
        }
    }
}