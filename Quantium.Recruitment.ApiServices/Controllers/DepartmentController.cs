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
    public class DepartmentController : ApiController
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentController(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        [HttpGet]
        public IHttpActionResult GetAll()
        {
            var departments = _departmentRepository.GetAll().ToList();

            var dDtos = Mapper.Map<List<DepartmentDto>>(departments);

            return Ok(dDtos);
        }

        [HttpGet]
        public IHttpActionResult GetSingle(int key)
        {
            var department = _departmentRepository.GetAll().SingleOrDefault(item => item.Id == key);

            return Ok(Mapper.Map<DepartmentDto>(department));
        }

        [HttpPost]
        public IHttpActionResult Create(DepartmentDto departmentDto)
        {
            var department = Mapper.Map<Department>(departmentDto);

            _departmentRepository.Add(department);

            return Ok(Mapper.Map<DepartmentDto>(department));
        }
    }
}