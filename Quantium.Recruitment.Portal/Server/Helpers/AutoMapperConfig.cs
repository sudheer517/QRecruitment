using AutoMapper;
using AutoMapper.Configuration;
using Quantium.Recruitment.Models;
using Quantium.Recruitment.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Quantium.Recruitment.Portal
{
    public static class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
            var cfg = new MapperConfigurationExpression();

            cfg.CreateMap<Question, QuestionDto>().ReverseMap();
            cfg.CreateMap<Option, OptionDto>().ReverseMap();
            cfg.CreateMap<QuestionGroup, QuestionGroupDto>().ReverseMap();

            cfg.CreateMap<Job, JobDto>().ReverseMap();
            cfg.CreateMap<Job_Difficulty_Label, Job_Difficulty_LabelDto>().ReverseMap();
            cfg.CreateMap<Label, LabelDto>().ReverseMap();
            cfg.CreateMap<Difficulty, DifficultyDto>().ReverseMap();
            cfg.CreateMap<Department, DepartmentDto>().ReverseMap();
            cfg.CreateMap<Admin, AdminDto>().ReverseMap();

            cfg.CreateMap<Candidate, CandidateDto>();
            cfg.CreateMap<CandidateDto, Candidate>().ForMember((m => m.Id), c => c.Ignore());

            cfg.CreateMap<Candidate_Job, Candidate_JobDto>().ReverseMap();

            cfg.CreateMap<Test, TestDto>().ReverseMap();
            cfg.CreateMap<Challenge, ChallengeDto>().ReverseMap();
            cfg.CreateMap<CandidateSelectedOption, CandidateSelectedOptionDto>().ReverseMap();
            cfg.CreateMap<Feedback, FeedbackDto>().ReverseMap();
            cfg.CreateMap<FeedbackType, FeedbackTypeDto>().ReverseMap();

            //cfg.CreateMap<Admin, AdminDto>();
            //cfg.CreateMap<AdminDto, Admin>();
            //cfg.CreateMap<Candidate, CandidateDto>();
            //cfg.CreateMap<CandidateSelectedOption, CandidateSelectedOptionDto>();
            //cfg.CreateMap<Challenge, ChallengeDto>();
            //cfg.CreateMap<Department, DepartmentDto>();
            //cfg.CreateMap<Job, JobDto>();
            //cfg.CreateMap<Label, LabelDto>();
            //cfg.CreateMap<Question, QuestionDto>();
            //cfg.CreateMap<QuestionDto, Question>().Ignore(m => m.Options);
            //cfg.CreateMap<List<Question>, List<QuestionDto>>();
            //cfg.CreateMap<Option, OptionDto>().ReverseMap();
            ////cfg.CreateMap<OptionDto, Option>().Ignore(m => m.Id);
            //cfg.CreateMap<List<Option>, List<OptionDto>>();
            //cfg.CreateMap<QuestionGroup, QuestionGroupDto>().ReverseMap();
            ////cfg.CreateMap<QuestionGroupDto, QuestionGroup>().Ignore(m => m.Id);
            //cfg.CreateMap<Test, TestDto>();

            Mapper.Initialize(cfg);
        }

        public static IMappingExpression<TSource, TDestination> Ignore<TSource, TDestination>(
            this IMappingExpression<TSource, TDestination> map,
            Expression<Func<TDestination, object>> selector)
        {
            map.ForMember(selector, config => config.Ignore());
            return map;
        }
    }
}