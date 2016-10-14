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
    public class LabelController : ApiController
    {
        private readonly ILabelRepository _labelRepository;

        public LabelController(ILabelRepository labelRepository)
        {
            _labelRepository = labelRepository;
        }

        [HttpGet]
        public IHttpActionResult GetAll()
        {
            var labels = _labelRepository.GetAll().ToList();

            var lDtos = Mapper.Map<List<LabelDto>>(labels);

            return Ok(lDtos);
        }

        [HttpGet]
        public IHttpActionResult GetSingle(int key)
        {
            var label = _labelRepository.GetAll().SingleOrDefault(item => item.Id == key);

            return Ok(Mapper.Map<LabelDto>(label));
        }
    }
}