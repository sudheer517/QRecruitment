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

        //    //http://localhost:60606/odata/Admins(1)
        //    [HttpGet]
        //    [ODataRoute("Admins({key})")]
        //    [EnableQuery]
        //    public IHttpActionResult GetAdmin([FromODataUri] int key)
        //    {
        //        var admin = _adminRepository.GetAll().Single(item => item.Id == key);

        //        return Ok(Mapper.Map<AdminDto>(admin));
        //    }

        //    //http://localhost:60606/odata/Admins
        //    [HttpPost]
        //    [ODataRoute("Admins")]
        //    [EnableQuery]
        //    public IHttpActionResult Post(AdminDto adminDto)
        //    {
        //        var admin = Mapper.Map<Admin>(adminDto);

        //        var result = _adminRepository.Add(admin);

        //        return Created(Mapper.Map<AdminDto>(result));
        //    }

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