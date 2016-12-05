using Microsoft.Practices.Unity;
using System;

namespace Quantium.Recruitment.DbInitialize
{
    class Initializer
    {
        static void Main(string[] args)
        {
            var container = IocContainer.GetContainer();
            var dataSeeder = container.Resolve<IDataSeeder>();

            Console.WriteLine("Beginning database initialization");

            dataSeeder.DeleteEntries();
            dataSeeder.Seed();
            Console.WriteLine("DB initialization finished. Press any key to exit !");
            Console.ReadKey();

        }
    }
}
