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
using Quantium.Recruitment.ApiServices.Helpers;
using System.Web.OData;
using System.Web.OData.Routing;

namespace Quantium.Recruitment.ApiServices.Controllers
{
    //[Authorize]
    public class OptionController : ApiController
    {
        private readonly IOptionRepository _optionRepository;

        public OptionController(IOptionRepository optionRepository)
        {
            _optionRepository = optionRepository;
        }

        [HttpGet]
        public IHttpActionResult Get()
        {
            var options = _optionRepository.GetAll().ToList();

            var oDtos = Mapper.Map<IList<QuestionDto>>(options);
            
            return Ok(oDtos);
        }

        [HttpGet]
        public IHttpActionResult GetSingle( int key)
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