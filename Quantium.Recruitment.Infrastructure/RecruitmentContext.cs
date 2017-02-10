using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Quantium.Recruitment.Entities;
using System.Data.Entity.Infrastructure;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quantium.Recruitment.Infrastructure
{
    public interface IRecruitmentContext : IDisposable
    {
        DbSet<Admin> Admins { get; set; }

        DbSet<Candidate> Candidates { get; set; }

        DbSet<Candidate_Job> CandidateJobs { get; set; }

        DbSet<Job> Jobs { get; set; }

        DbSet<Department> Departments { get; set; }

        DbSet<Question> Questions { get; set; }

        DbSet<Option> Options { get; set; }

        DbSet<QuestionGroup> QuestionGroups { get; set; }

        DbSet<Test> Tests { get; set; }

        DbSet<Label> Labels { get; set; }

        DbSet<Difficulty> Difficulties { get; set; }

        DbSet<Challenge> Challenges { get; set; }

        DbSet<CandidateSelectedOption> CandidateSelectedOptions { get; set; }

        DbSet<Job_Difficulty_Label> JobDifficultyLabels { get; set; }

        DbSet<Survey> Surveys { get; set; }

        DbSet<SurveyChallenge> SurveyChallenges { get; set; }

        DbSet<SurveyQuestion> SurveyQuestions { get; set; }

        int SaveChanges();

        Database GetDatabase();

        DbSet GetSet(Type entityType);

        DbEntityEntry Entry(object entity);
    }

    public class RecruitmentContext : DbContext, IRecruitmentContext
    {
        public RecruitmentContext(IConnectionString connectionString)
        {
            this.Database.Connection.ConnectionString = connectionString.GetConnectionString();
            this.Database.CreateIfNotExists();
        }

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

        public Database GetDatabase()
        {
            return this.Database;
        }

        public DbSet GetSet(Type entityType)
        {
            return Set(entityType);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }
    }
}
