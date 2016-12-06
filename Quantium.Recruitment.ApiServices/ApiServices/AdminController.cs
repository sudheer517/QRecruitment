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

namespace Quantium.Recruitment.ApiServices.Controllers
{
    //[Authorize]
    public class AdminController : ApiController
    {
        private readonly ICandidateRepository _candidateRepository;
        private readonly IAdminRepository _adminRepository;

        public AdminController(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        [HttpGet]
        public IHttpActionResult IsAdmin([FromUri]string email)
        {
            var admin = _adminRepository.GetAll().SingleOrDefault(a => a.Email == email && a.IsActive == true);

            return Ok(Mapper.Map<AdminDto>(admin));
        }

        [HttpGet]
        public IHttpActionResult GetAdmin(int key)
        {
            var admin = _adminRepository.GetAll().Single(item => item.Id == key);

            return Ok(Mapper.Map<AdminDto>(admin));
        }

        [HttpPost]
        public HttpResponseMessage AddAdmin(AdminDto adminDto)
        {
            var admin = Mapper.Map<Admin>(adminDto);

            var result = _adminRepository.Add(admin);

            return Request.CreateResponse(HttpStatusCode.Created);
        }
    }
}