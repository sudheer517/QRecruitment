using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.OData.Extensions;
using AutoMapper;
using Microsoft.OData.Edm;
using Quantium.Recruitment.ApiServices.Models;
using Quantium.Recruitment.Entities;
using System.Web.OData.Builder;

namespace Quantium.Recruitment.ApiServices
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapODataServiceRoute("ODataRoute", "odata", GetEDMModel());
            config.EnsureInitialized();
        }

        private static IEdmModel GetEDMModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.Namespace = "QRecruitment";
            builder.ContainerName = "QRecruitmentContainer";

            builder.EntitySet<AdminDto>("Admins");
            builder.EntitySet<CandidateDto>("Candidates");
            builder.EntitySet<CandidateSelectedOptionDto>("CandidateSelectedOptionDto");
            builder.EntitySet<ChallengeDto>("ChallengeDto");
            builder.EntitySet<DepartmentDto>("DepartmentDto");
            builder.EntitySet<JobDto>("JobDto");
            builder.EntitySet<LabelDto>("LabelDto");
            builder.EntitySet<OptionDto>("OptionDto");
            builder.EntitySet<QuestionDto>("Questions");
            builder.EntitySet<QuestionGroupDto>("QuestionGroupDto");
            builder.EntitySet<TestDto>("TestDto");

            return builder.GetEdmModel();
        }
    }
}
