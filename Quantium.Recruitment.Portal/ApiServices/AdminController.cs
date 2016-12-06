using Quantium.Recruitment.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using AutoMapper;
using Quantium.Recruitment.ApiServices.Models;
using Quantium.Recruitment.Entities;
using Quantium.Recruitment.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace Quantium.Recruitment.ApiServices.Controllers
{
    //[Authorize]
    [Route("api/admin")]
    public class AdminController : Controller
    {
        private readonly ICandidateRepository _candidateRepository;
        private readonly IAdminRepository _adminRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AdminController(IAdminRepository adminRepository, IHttpContextAccessor httpContextAccessor)
        {
            _adminRepository = adminRepository;
            _httpContextAccessor = httpContextAccessor;
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
            var admin = _adminRepository.GetAll().Single(item => item.Id == key);

            return Ok(Mapper.Map<AdminDto>(admin));
        }

        [HttpPost]
        public HttpResponseMessage AddAdmin(AdminDto adminDto)
        {
            var admin = Mapper.Map<Admin>(adminDto);

            var result = _adminRepository.Add(admin);

            return new HttpResponseMessage(HttpStatusCode.Created);
        }

        //    //http://localhost:60606/odata/Admins(1)
        //    [HttpDelete]
        //    [ODataRoute("Admins({key})")]
        //    [EnableQuery]
        //    public void DeleteAdmin([FromODataUri] int key)
        //    {
        //        var admin = _adminRepository.GetAll().Single(item => item.Id == key);

        //        if (admin != null)
        //        {
        //            _adminRepository.Delete(admin);
        //        }
        //    }

        //    //http://localhost:60606/odata/Admins
        //    [HttpPatch]
        //    [ODataRoute("Admins")]
        //    [EnableQuery]
        //    public void Patch([FromODataUri] int key,AdminDto adminDto)
        //    {
        //        var admin = Mapper.Map<Admin>(adminDto);

        //        _adminRepository.Update(admin);
        //    }

    }
}