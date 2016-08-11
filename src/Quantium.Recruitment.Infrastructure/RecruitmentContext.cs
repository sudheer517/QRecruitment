using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using Quantium.Recruitment.Entities;

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
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }

    public class RecruitmentContext : DbContext, IRecruitmentContext
    {
        private IConfigurationRoot _config;

        public RecruitmentContext(IConfigurationRoot config, DbContextOptions options): base(options)
        {
            _config = config;
            this.Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(_config["ConnectionStrings:RecruitmentContextConnection"]);

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (IMutableEntityType entity in modelBuilder.Model.GetEntityTypes())
            {
                entity.Relational().TableName = entity.Name;
            }

            modelBuilder.Entity<Candidate>()
                .HasOne(i => i.Job)
                .WithMany(i => i.Candidates)
                .HasForeignKey(i => i.JobId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Candidate>()
                .HasOne(i => i.Test)
                .WithMany(i => i.Candidates)
                .HasForeignKey(i => i.TestId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Job>()
                .HasOne(i => i.Department)
                .WithMany(i => i.Jobs)
                .HasForeignKey(i => i.DepartmentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Admin>()
                .HasOne(i => i.Department)
                .WithMany(i => i.Admins)
                .HasForeignKey(i => i.DepartmentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Label>()
                .HasOne(i => i.Job)
                .WithMany(i => i.Labels)
                .HasForeignKey(i => i.JobId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Question>()
                .HasOne(i => i.QuestionGroup)
                .WithMany(i => i.Questions)
                .HasForeignKey(i => i.QuestionGroupId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Option>()
                .HasOne(i => i.Question)
                .WithMany(i => i.Options)
                .HasForeignKey(i => i.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Challenge>()
                .HasOne(i => i.Question)
                .WithMany(i => i.Challenges)
                .HasForeignKey(i => i.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Challenge>()
                .HasOne(i => i.Test)
                .WithMany(i => i.Challenges)
                .HasForeignKey(i => i.TestId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CandidateSelectedOption>()
                .HasOne(i => i.Challenge)
                .WithMany(i => i.CandidateSelectedOptions)
                .HasForeignKey(i => i.ChallengeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Label>()
                .HasOne(i => i.Test)
                .WithMany(i => i.Labels)
                .HasForeignKey(i => i.TestId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
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
