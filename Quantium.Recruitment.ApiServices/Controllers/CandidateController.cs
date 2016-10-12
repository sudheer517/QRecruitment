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
using System.Web.OData;
using System.Web.OData.Routing;

namespace Quantium.Recruitment.ApiServices.Controllers
{
    //[Authorize]
    //public class CandidateController : ODataController
    //{
    //    private readonly ICandidateRepository _candidateRepository;
    //    //private readonly IAdminRepository _adminRepository;

    //    //public UsersController(ICandidateRepository candidateRepository, IAdminRepository adminRepository)
    //    //{
    //    //    _candidateRepository = candidateRepository;
    //    //    _adminRepository = adminRepository;
    //    //}

    //    public CandidateController(ICandidateRepository candidateRepository)
    //    {
    //        _candidateRepository = candidateRepository;
    //    }

    //    //http://localhost:60606/odata/Candidates
    //    [HttpGet]
    //    [ODataRoute("Candidates")]
    //    [EnableQuery]
    //    public IHttpActionResult GetCandidates()
    //    {
    //        var candidates = _candidateRepository.GetAll().ToList();

    //        return Ok(Mapper.Map<IList<CandidateDto>>(candidates));
    //    }

    //    //http://localhost:60606/odata/Candidates(1)
    //    [HttpGet]
    //    [ODataRoute("Candidates({key})")]
    //    public IHttpActionResult GetCandidate([FromODataUri] int key)
    //    {
    //        var candidate = _candidateRepository.GetAll().Single(item => item.Id == key);

    //        return Ok(Mapper.Map<CandidateDto>(candidate));
    //    }

    //    ////http://localhost:60606/odata/Admins
    //    //[HttpGet]
    //    //[ODataRoute("Admins")]
    //    //public IHttpActionResult GetAdmins()
    //    //{
    //    //    var admins = _adminRepository.GetAll().ToList();

    //    //    return Ok(Mapper.Map<IList<AdminDto>>(admins));
    //    //}

    //    ////http://localhost:60606/odata/Admins(1)
    //    //[HttpGet]
    //    //[ODataRoute("Admins({key})")]
    //    //public IHttpActionResult GetAdmin([FromODataUri] int key)
    //    //{
    //    //    var admin = _adminRepository.GetAll().Single(item => item.Id == key);

    //    //    return Ok(Mapper.Map<AdminDto>(admin));
    //    //}
    //}
}