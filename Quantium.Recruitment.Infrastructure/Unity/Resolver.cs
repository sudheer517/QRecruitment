using System.Collections.Generic;
using Microsoft.Practices.Unity;

namespace Quantium.Recruitment.Infrastructure.Unity
{
    public interface IResolver<T>
    {
        T Resolve();

        IEnumerable<T> ResolveAll();

        T Resolve(string name);
    }

    public class Resolver<T>: IResolver<T>
    {
        private readonly IUnityContainer _container;
        public Resolver(IUnityContainer container)
        {
            _container = container;
        }
        
        public T Resolve()
        {
            return _container.Resolve<T>();
        }

        public IEnumerable<T> ResolveAll()
        {
            return _container.ResolveAll<T>();
        }

        public T Resolve(string name)
        {
            return _container.Resolve<T>(name);
        }
    }
}
