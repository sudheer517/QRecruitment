using Microsoft.Practices.Unity;
using Quantium.Recruitment.Infrastructure;
using System.Web.Http;
using Unity.WebApi;

namespace Quantium.Recruitment.ApiServices
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            container.RegisterType<IRecruitmentContext, RecruitmentContext>();
            // register all your components with the container here
            // it is NOT necessary to register your controllers
            
            // e.g. container.RegisterType<ITestService, TestService>();
            
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}