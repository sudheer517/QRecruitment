using Microsoft.Practices.Unity;
using Quantium.Recruitment.Infrastructure;
using System.Web.Http;
using Quantium.Recruitment.Infrastructure.Repositories;
using Unity.WebApi;
using System.Web.Http.Dependencies;

namespace Quantium.Recruitment.ApiServices
{
    public static class UnityConfig
    {
        public static IDependencyResolver RegisterComponents()
        {
			var container = new UnityContainer();
            container.RegisterType<IRecruitmentContext, RecruitmentContext>();
            container.RegisterType<IQuestionRepository, QuestionRepository>();
            container.RegisterType<IConnectionString, ConnectionString>();
            container.RegisterType<ICandidateRepository, CandidateRepository>();
            container.RegisterType<IAdminRepository, AdminRepository>();

            return new UnityDependencyResolver(container);
        }
    }
}