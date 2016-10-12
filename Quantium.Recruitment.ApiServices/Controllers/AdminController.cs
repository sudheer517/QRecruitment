﻿using Quantium.Recruitment.Infrastructure;
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
using System.Web.OData;
using System.Web.OData.Routing;

namespace Quantium.Recruitment.ApiServices.Controllers
{
    //[Authorize]
    //public class AdminController : ODataController
    //{
    //    private readonly ICandidateRepository _candidateRepository;
    //    private readonly IAdminRepository _adminRepository;

    //    public AdminController(IAdminRepository adminRepository)
    //    {
    //        _adminRepository = adminRepository;
    //    }

    //    //http://localhost:60606/odata/Admins
    //    [HttpGet]
    //    [ODataRoute("Admins")]
    //    [EnableQuery]
    //    public IHttpActionResult GetAdmins()
    //    {
    //        var admins = _adminRepository.GetAll().ToList();

    //        return Ok(Mapper.Map<IList<AdminDto>>(admins));
    //    }

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

    //}
}