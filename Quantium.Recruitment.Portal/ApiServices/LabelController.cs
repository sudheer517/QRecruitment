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
    [Route("api/label")]
    public class LabelController : Controller
    {
        private readonly ILabelRepository _labelRepository;

        public LabelController(ILabelRepository labelRepository)
        {
            _labelRepository = labelRepository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var labels = _labelRepository.GetAll().ToList();

            var lDtos = Mapper.Map<List<LabelDto>>(labels);

            return Ok(lDtos);
        }

        [HttpGet]
        public IActionResult GetSingle(int key)
        {
            var label = _labelRepository.GetAll().SingleOrDefault(item => item.Id == key);

            return Ok(Mapper.Map<LabelDto>(label));
        }
    }
}