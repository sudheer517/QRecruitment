using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Quantium.Recruitment.Entities;

namespace Quantium.Recruitment.Infrastructure.RDbContext
{
    public interface IRecruitmentContext : IDisposable
    {
        DbSet<Admin> Admins { get; set; }

        DbSet<Candidate> Candidates { get; set; }

        DbSet<Job> Jobs { get; set; }

        DbSet<Department> Departments { get; set; }

        DbSet<Question> Questions { get; set; }

        DbSet<Option> Options { get; set; }

        DbSet<QuestionGroup> QuestionGroups { get; set; }

        DbSet<Test> Tests { get; set; }

        DbSet<Label> Labels { get; set; }

        DbSet<Challenge> Challenges { get; set; }

        DbSet<CandidateSelectedOption> CandidateSelectedOptions { get; set; }

        int SaveChanges();
    }

    public class RecruitmentContext : DbContext, IRecruitmentContext
    {
        private IConfigurationRoot _config;

        public RecruitmentContext(IConfigurationRoot config)
        {
            _config = config;
            this.Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(_config["ConnectionStrings:RecruitmentContextConnection"]);
        }

        public DbSet<Admin> Admins { get; set; }

        public DbSet<Candidate> Candidates { get; set; }

        public DbSet<Job> Jobs { get; set; }

        public DbSet<Department> Departments { get; set; }

        public DbSet<Question> Questions { get; set; }

        public DbSet<Option> Options { get; set; }

        public DbSet<QuestionGroup> QuestionGroups { get; set; }

        public DbSet<Test> Tests { get; set; }

        public DbSet<Label> Labels { get; set; }

        public DbSet<Challenge> Challenges { get; set; }

        public DbSet<CandidateSelectedOption> CandidateSelectedOptions { get; set; }
    }
}
