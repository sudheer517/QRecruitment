using System;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Quantium.Recruitment.Infrastructure;
using Quantium.Recruitment.Entities;
using Quantium.Recruitment.Infrastructure.IoCContainer;
using Quantium.Recruitment.Infrastructure.RDbContext;

namespace Quantium.Recruitment.DbInitialize
{
    public class Program
    {
        public static IRecruitmentContext _dbContext { get; set; }

        private static IConfigurationRoot _configuration { get; set; }

        public static void Main(string[] args)
        {
            Console.WriteLine("Creating database...");

            _configuration = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json")
                   .Build();

            _dbContext = IoCContainer.ConfigureServices(_configuration).BuildServiceProvider().GetService<IRecruitmentContext>();

            Console.WriteLine("Seeding data...");

            SeedData();

            Console.WriteLine("Database initialized.");
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        public static void SeedData()
        {
            Department softwareDepartment = new Department() { Name = "Software" };
            Department analyticsDepartment = new Department() { Name = "Analytics" };

            _dbContext.Departments.Add(softwareDepartment);
            _dbContext.Departments.Add(analyticsDepartment);

            Admin admin = new Admin()
            {
                FirstName = "Kannan",
                LastName = "Perumal",
                Email = "kannan.perumal@quantium.co.in",
                IsActive = true,
                Mobile = 8886008855,
                Department = softwareDepartment
            };

            _dbContext.Admins.Add(admin);
            _dbContext.SaveChanges();
        }
    }
}
