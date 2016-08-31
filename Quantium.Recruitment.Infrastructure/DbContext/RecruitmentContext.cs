using System;
using Quantium.Recruitment.Entities;
using System.Data.Entity;

namespace Quantium.Recruitment.Infrastructure
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

        public RecruitmentContext() : base()
        {
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
