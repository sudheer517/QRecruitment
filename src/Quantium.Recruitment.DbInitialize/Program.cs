using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quantium.Recruitment.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quantium.Recruitment.DbInitialize
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //IServiceCollection serviceCollection = new ServiceCollection();
            //serviceCollection.AddTransient<IRecruitmentContext, RecruitmentContext>();
            
            //var p = new DBRunner();
        }

        #region commented
        //private static void InitializeDb()
        //{
        //    Console.WriteLine("DB Initialization starting...");

        //    Console.WriteLine("Creating database...");

        //    InitializeEntities();

        //    Console.WriteLine("All done, have fun !!!!");
        //}

        //private static void InitializeEntities()
        //{
        //    using (var dbContext = IocContainer.Container.Resolve<IRecruitmentContext>())
        //    {
        //        Department softwareDepartment = new Department() { Name = "Software" };
        //        Department analyticsDepartment = new Department() { Name = "Analytics" };

        //        dbContext.Departments.Add(softwareDepartment);
        //        dbContext.Departments.Add(analyticsDepartment);

        //        Admin admin = new Admin()
        //        {
        //            FirstName = "Kannan",
        //            LastName = "Perumal",
        //            Email = "kannan.perumal@quantium.co.in",
        //            IsActive = true,
        //            Mobile = 8886008855,
        //            Department = softwareDepartment
        //        };

        //        dbContext.Admins.Add(admin);
        //        dbContext.SaveChanges();
        //    }
        //}
        #endregion
    }
}
