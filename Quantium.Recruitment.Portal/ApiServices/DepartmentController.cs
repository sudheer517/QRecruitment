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

namespace Quantium.Recruitment.ApiServices.Controllers
{
    [Route("api/[controller]/[action]/{id?}")]
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentController(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var departments = _departmentRepository.GetAll().ToList();

            var dDtos = Mapper.Map<List<DepartmentDto>>(departments);

            return Ok(dDtos);
        }

        [HttpGet]
        public IActionResult GetSingle(int key)
        {
            var department = _departmentRepository.GetAll().SingleOrDefault(item => item.Id == key);

            return Ok(Mapper.Map<DepartmentDto>(department));
        }

        [HttpPost]
        public IActionResult Create(DepartmentDto departmentDto)
        {
            var department = Mapper.Map<Department>(departmentDto);

            _departmentRepository.Add(department);

            return Ok(Mapper.Map<DepartmentDto>(department));
        }
    }
}