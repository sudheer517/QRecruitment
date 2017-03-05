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

namespace Quantium.Recruitment.ApiServices.Controllers
{
    //[Authorize]
    //[Route("api/admin")]
    //[Route("api/[controller]/[action]/{id?}")]
    [Route("[controller]/[action]/{id?}")]
    public class AdminController : Controller
    {
        private readonly IEntityBaseRepository<Admin> _adminRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AdminController(IEntityBaseRepository<Admin> adminRepository, IHttpContextAccessor httpContextAccessor)
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
        public IActionResult AddAdmin([FromBody]AdminDto adminDto)
        {
            var admin = Mapper.Map<Admin>(adminDto);

            try
            {
                _adminRepository.Add(admin);
                return Created("created", admin);
            }
            catch(Exception ex)
            {
                return BadRequest("unable to add admin");
            }
        }
    }
}