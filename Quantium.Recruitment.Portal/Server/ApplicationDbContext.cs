using AspNetCoreSpa.Server.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Quantium.Recruitment.Entities;
using Quantium.Recruitment.Portal.Server.Entities;
using System.Linq;

namespace AspNetCoreSpa.Server
{
    public class ApplicationDbContext : IdentityDbContext<QRecruitmentUser, QRecruitmentRole, int>
    {
        public DbSet<QRecruitmentUser> ApplicationUsers { get; set; }

        public DbSet<QRecruitmentRole> ApplicationRoles { get; set; }
        
        public DbSet<Admin> Admins { get; set; }

        public DbSet<Candidate> Candidates { get; set; }

        public DbSet<Candidate_Job> CandidateJobs { get; set; }

        public DbSet<Job> Jobs { get; set; }

        public DbSet<Department> Departments { get; set; }

        public DbSet<Question> Questions { get; set; }

        public DbSet<Option> Options { get; set; }

        public DbSet<QuestionGroup> QuestionGroups { get; set; }

        public DbSet<Test> Tests { get; set; }

        public DbSet<Label> Labels { get; set; }

        public DbSet<Difficulty> Difficulties { get; set; }

        public DbSet<Challenge> Challenges { get; set; }

        public DbSet<CandidateSelectedOption> CandidateSelectedOptions { get; set; }

        public DbSet<Job_Difficulty_Label> JobDifficultyLabels { get; set; }

        public DbSet<Survey> Surveys { get; set; }

        public DbSet<SurveyChallenge> SurveyChallenges { get; set; }

        public DbSet<SurveyQuestion> SurveyQuestions { get; set; }

        public DbSet<FeedbackType> FeedbackTypes { get; set; }

        public DbSet<Feedback> Feedbacks { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Content
            //foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            //{
            //    relationship.DeleteBehavior = DeleteBehavior.Cascade;
            //}

            modelBuilder.Entity<Option>()
                .HasOne(p => p.CandidateSelectedOption)
                .WithOne(b => b.Option)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Admin>()
                .HasMany(cj => cj.Candidates)
                .WithOne(c => c.Admin)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.Test)
                .WithOne(t => t.Feedback)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

        }
    }
}
