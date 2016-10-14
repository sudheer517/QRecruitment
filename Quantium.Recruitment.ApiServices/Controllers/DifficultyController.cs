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
    public class DifficultyController : ApiController
    {
        private readonly IDifficultyRepository _difficultyRepository;

        public DifficultyController(IDifficultyRepository difficultyRepository)
        {
            _difficultyRepository = difficultyRepository;
        }

        [HttpGet]
        public IHttpActionResult GetAll()
        {
            var difficulties = _difficultyRepository.GetAll().ToList();

            var dDtos = Mapper.Map<List<DifficultyDto>>(difficulties);

            return Ok(dDtos);
        }

        [HttpGet]
        public IHttpActionResult GetSingle(int key)
        {
            var difficulty = _difficultyRepository.GetAll().SingleOrDefault(item => item.Id == key);

            return Ok(Mapper.Map<DifficultyDto>(difficulty));
        }
    }
}