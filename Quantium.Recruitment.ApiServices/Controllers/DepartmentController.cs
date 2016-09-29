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
    public class DepartmentController : ODataController
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentController(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        //http://localhost:60606/odata/Departments
        [HttpGet]
        [ODataRoute("DepartmentDto")]
        [EnableQuery]
        public IHttpActionResult GetDepartments()
        {
            var departments = _departmentRepository.GetAll().ToList();

            return Ok(Mapper.Map<IList<DepartmentDto>>(departments));
        }
    }
}