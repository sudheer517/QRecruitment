using Microsoft.Practices.Unity;
using Quantium.Recruitment.Infrastructure;
using System.Web.Http;
using Quantium.Recruitment.Infrastructure.Repositories;
using Unity.WebApi;

namespace Quantium.Recruitment.ApiServices
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            container.RegisterType<IRecruitmentContext, RecruitmentContext>();
            container.RegisterType<IQuestionRepository, QuestionRepository>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}