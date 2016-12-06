using Quantium.Recruitment.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using Quantium.Recruitment.ApiServices.Models;
using Quantium.Recruitment.Entities;
using Quantium.Recruitment.Infrastructure.Repositories;
using System.Web;
using System.IO;
using Excel;
using System.Data;
using ClosedXML.Excel;
using System.ComponentModel.DataAnnotations;

namespace Quantium.Recruitment.ApiServices.Controllers
{
    public class CandidateController : ApiController
    {
        private readonly ICandidateRepository _candidateRepository;

        public CandidateController(ICandidateRepository candidateRepository)
        {
            _candidateRepository = candidateRepository;
        }

        [HttpGet]
        public IHttpActionResult GetAllCandidates()
        {
            var candidates = _candidateRepository.GetAll().Where(candidate => candidate.IsActive).ToList();

            return Ok(Mapper.Map<IList<CandidateDto>>(candidates));
        }

        [HttpGet]
        public IHttpActionResult GetCandidatesWithoutActiveTests()
        {
            var candidates = _candidateRepository.GetAll().Where(c => c.IsActive && c.Tests.Count() == 0).ToList();

            return Ok(Mapper.Map<IList<CandidateDto>>(candidates));
        }

        [HttpGet]
        public IHttpActionResult GetCandidateByEmail([FromUri]string email)
        {
            var candidate = _candidateRepository.GetAll().SingleOrDefault(item => item.Email == email && item.IsActive == true);

            if(candidate != null)
                return Ok(Mapper.Map<CandidateDto>(candidate));
            else
                return Ok();
        }

        [HttpGet]
        public IHttpActionResult IsInformationFilled([FromUri]string email)
        {
            var candidate = _candidateRepository.GetAll().SingleOrDefault(item => item.Email == email && item.IsActive == true && item.IsInformationFilled == true);

            if (candidate != null)
                return Ok(true);
            else
                return Ok(false);
        }

        [HttpGet]
        public IHttpActionResult GetSingleCandidate(int key)
        {
            var candidate = _candidateRepository.GetAll().Single(item => item.Id == key);

            return Ok(Mapper.Map<CandidateDto>(candidate));
        }

        [HttpGet]
        public IHttpActionResult GetCandidateName([FromUri]string email)
        {
            var candidate = _candidateRepository.GetAll().Single(item => item.Email == email);

            return Ok($"{candidate.FirstName} {candidate.LastName}");
        }

        private List<CandidateDto> ParseInputCandidateFile()
        {
            var httpRequest = HttpContext.Current.Request;

            var streamResult = Request.Content.ReadAsStreamAsync().Result;

            List<CandidateDto> candidateDtos = new List<CandidateDto>();
            using (var ms = new MemoryStream())
            {
                httpRequest.InputStream.CopyToAsync(ms);
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
                            throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, message));
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
        public IHttpActionResult Add([FromBody]List<CandidateDto> candidateDtos)
        {
            var candidates = Mapper.Map<List<Candidate>>(candidateDtos);

            foreach (var candidate in candidates)
            {
                candidate.IsActive = true;
                _candidateRepository.Add(candidate);
            }

            return Created(string.Empty, "Candidates Created");
        }

        [HttpPost]
        public IHttpActionResult AddCandidates()
        {
            var candidateDtos = ParseInputCandidateFile();

            var candidates = Mapper.Map<List<Candidate>>(candidateDtos);

            foreach (var candidate in candidates)
            {
                candidate.IsActive = true;
                _candidateRepository.Add(candidate);
            }

            return Created(string.Empty, "Candidates Created");
        }

        [HttpPost]
        public IHttpActionResult PreviewCandidates()
        {
            var candidateDtos = ParseInputCandidateFile();
            return Ok(candidateDtos);
        }

        [HttpPost]
        public IHttpActionResult FillCandidateInformation([FromBody]CandidateDto candidateDto)
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

        ////http://localhost:60606/odata/Admins
        //[HttpGet]
        //[ODataRoute("Admins")]
        //public IHttpActionResult GetAdmins()
        //{
        //    var admins = _adminRepository.GetAll().ToList();

        //    return Ok(Mapper.Map<IList<AdminDto>>(admins));
        //}

        ////http://localhost:60606/odata/Admins(1)
        //[HttpGet]
        //[ODataRoute("Admins({key})")]
        //public IHttpActionResult GetAdmin([FromODataUri] int key)
        //{
        //    var admin = _adminRepository.GetAll().Single(item => item.Id == key);

        //    return Ok(Mapper.Map<AdminDto>(admin));
        //}
    }
}