using Quantium.Recruitment.Entities;
using Quantium.Recruitment.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quantium.Recruitment.Services
{
    public class RecruitmentContextSeedData
    {
        private RecruitmentContext _context;

        public RecruitmentContextSeedData(RecruitmentContext context)
        {
            _context = context;
        }

        public async Task EnsureSeedData()
        {
            if (!_context.Departments.Any())
            {
                Department softwareDepartment = new Department() { Name = "Software" };
                Department analyticsDepartment = new Department() { Name = "Analytics" };

                _context.Departments.Add(softwareDepartment);
                _context.Departments.Add(analyticsDepartment);

                Admin admin = new Admin()
                {
                    FirstName = "Kannan",
                    LastName = "Perumal",
                    Email = "kannan.perumal@quantium.co.in",
                    IsActive = true,
                    Mobile = 8886008855,
                    Department = softwareDepartment
                };

                _context.Admins.Add(admin);
                //_context.SaveChanges();
                await _context.SaveChangesAsync();
            }
        }
    }
}
