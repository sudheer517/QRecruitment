using Microsoft.Practices.Unity;
using Quantium.Recruitment.Infrastructure;
using System.Web.Http;
using Unity.WebApi;

namespace Quantium.Recruitment.Services
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			IUnityContainer container = new UnityContainer();

            container.RegisterType<IRecruitmentContext, RecruitmentContext>(new HierarchicalLifetimeManager());
            // register all your components with the container here
            // it is NOT necessary to register your controllers
            
            // e.g. container.RegisterType<ITestService, TestService>();
            
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}