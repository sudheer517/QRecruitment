using AutoMapper;
using AutoMapper.Configuration;
using Quantium.Recruitment.ApiServices.Models;
using Quantium.Recruitment.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quantium.Recruitment.ApiServices
{
    public static class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
            var cfg = new MapperConfigurationExpression();

            cfg.CreateMap<Admin, AdminDto>();
            cfg.CreateMap<Candidate, CandidateDto>();
            cfg.CreateMap<CandidateSelectedOption, CandidateSelectedOptionDto>();
            cfg.CreateMap<Challenge, ChallengeDto>();
            cfg.CreateMap<Department, DepartmentDto>();
            cfg.CreateMap<Job, JobDto>();
            cfg.CreateMap<Label, LabelDto>();
            cfg.CreateMap<Question, QuestionDto>();
            cfg.CreateMap<List<Question>, List<QuestionDto>>();
            cfg.CreateMap<Option, OptionDto>();
            cfg.CreateMap<QuestionGroup, QuestionGroupDto>();
            cfg.CreateMap<Test, TestDto>();

            Mapper.Initialize(cfg);
        }
    }
}