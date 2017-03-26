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
    public class DifficultyController : Controller
    {
        private readonly IEntityBaseRepository<Difficulty> _difficultyRepository;

        public DifficultyController(IEntityBaseRepository<Difficulty> difficultyRepository)
        {
            _difficultyRepository = difficultyRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var difficulties = await _difficultyRepository.GetAllAsync();

            var dDtos = Mapper.Map<List<DifficultyDto>>(difficulties.ToList());

            return Ok(dDtos);
        }

        [HttpGet]
        public IActionResult GetSingle(int key)
        {
            var difficulty = _difficultyRepository.GetAll().SingleOrDefault(item => item.Id == key);

            return Ok(Mapper.Map<DifficultyDto>(difficulty));
        }

        [HttpPost]
        public IActionResult Create(DifficultyDto difficultyDto)
        {
            var difficulty = Mapper.Map<Difficulty>(difficultyDto);

            _difficultyRepository.Add(difficulty);

            return Ok(Mapper.Map<DifficultyDto>(difficulty));
        }
    }
}