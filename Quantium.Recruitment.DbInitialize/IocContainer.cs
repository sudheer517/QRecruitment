using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Quantium.Recruitment.Infrastructure;
using Quantium.Recruitment.Infrastructure.Unity;

namespace Quantium.Recruitment.DbInitialize
{
    public static class IocContainer
    {
        private static readonly object Lock = new object();
        private static IUnityContainer _container;

        public static IUnityContainer GetContainer()
        {
            if (_container == null)
            {
                _container = new UnityContainer();
                DoRegistrations();
            }

            return _container;
        }

        private static void DoRegistrations()
        {
            RegisterTypes();
            RegisterInstances();
        }

        private static void RegisterTypes()
        {
            _container.RegisterType(typeof (IRecruitmentContext), typeof (RecruitmentContext));
            _container.RegisterType(typeof (IConnectionString), typeof (ConnectionString));
            _container.RegisterType(typeof (IDataSeeder), typeof (DataSeeder));
            _container.RegisterType(typeof(IResolver<>), typeof(Resolver<>), new InjectionConstructor(_container));
        }

        private static void RegisterInstances()
        {
        }
    }
}
