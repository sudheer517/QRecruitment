using Microsoft.EntityFrameworkCore;
using Quantium.Recruitment.Entities;
using Quantium.Recruitment.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quantium.Recruitment.DbInitialize
{
    public class DBRunner
    {
        private IRecruitmentContext _dbContext;

        public DBRunner(RecruitmentContext recruitmentContext)
        {
            _dbContext = recruitmentContext;
        }

        private void InitializeDb()
        {
            Console.WriteLine("DB Initialization starting...");

            Console.WriteLine("Creating database...");

            InitializeEntities();

            Console.WriteLine("All done, have fun !!!!");
        }

        private void InitializeEntities()
        {
            using (var dbContext = _dbContext)
            {
                Department softwareDepartment = new Department() { Name = "Software" };
                Department analyticsDepartment = new Department() { Name = "Analytics" };

                dbContext.Departments.Add(softwareDepartment);
                dbContext.Departments.Add(analyticsDepartment);

                Admin admin = new Admin()
                {
                    FirstName = "Kannan",
                    LastName = "Perumal",
                    Email = "kannan.perumal@quantium.co.in",
                    IsActive = true,
                    Mobile = 8886008855,
                    Department = softwareDepartment
                };

                dbContext.Admins.Add(admin);
                dbContext.SaveChanges();
            }
        }
    }
}
