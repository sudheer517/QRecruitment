using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Quantium.Recruitment.Portal.Models;

namespace Quantium.Recruitment.Portal.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, QRecruitmentRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            this.Database.EnsureCreated();
            this.Database.Migrate();
        }
    }
}
