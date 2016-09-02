using Microsoft.Practices.Unity;
using Quantium.Recruitment.Infrastructure;

namespace Quantium.Recruitment.DbInitialize
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = IocContainer.GetContainer();
            var dataSeeder = container.Resolve<IDataSeeder>();

            dataSeeder.Seed();
        }
    }
}
