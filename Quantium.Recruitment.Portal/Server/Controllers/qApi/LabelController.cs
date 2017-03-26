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
using AspNetCoreSpa.Server.Repositories.Abstract;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace Quantium.Recruitment.ApiServices.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("[controller]/[action]/{id?}")]
    public class LabelController : Controller
    {
        private readonly IEntityBaseRepository<Label> _labelRepository;

        public LabelController(IEntityBaseRepository<Label> labelRepository)
        {
            _labelRepository = labelRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var labels = await _labelRepository.GetAllAsync();

            var lDtos = Mapper.Map<List<LabelDto>>(labels.ToList());

            return Ok(lDtos);
        }

        [HttpGet]
        public IActionResult GetSingle(int key)
        {
            var label = _labelRepository.GetAll().SingleOrDefault(item => item.Id == key);

            return Ok(Mapper.Map<LabelDto>(label));
        }

        [HttpPost]
        public IActionResult Create(LabelDto labelDto)
        {
            var label = Mapper.Map<Label>(labelDto);

            _labelRepository.Add(label);

            return Ok(Mapper.Map<LabelDto>(label));
        }
    }
}