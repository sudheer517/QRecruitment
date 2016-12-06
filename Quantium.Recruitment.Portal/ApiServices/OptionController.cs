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
    //[Authorize]
    //[Route("api/option")]
    [Route("api/[controller]/[action]/{id?}")]
    public class OptionController : Controller
    {
        private readonly IOptionRepository _optionRepository;

        public OptionController(IOptionRepository optionRepository)
        {
            _optionRepository = optionRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var options = _optionRepository.GetAll().ToList();

            var oDtos = Mapper.Map<IList<QuestionDto>>(options);
            
            return Ok(oDtos);
        }

        [HttpGet]
        public IActionResult GetSingle( int key)
        {
            var option = _optionRepository.GetAll().Single(item => item.Id == key);

            return Ok(Mapper.Map<OptionDto>(option));
        }

        //[HttpPost]
        //public IHttpActionResult Post(OptionDto optionDto)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var inputOption = Mapper.Map<Option>(optionDto);

        //    var result = _optionRepository.Add(inputOption);

        //    return Created(Mapper.Map<OptionDto>(result));
        //}
    }
}