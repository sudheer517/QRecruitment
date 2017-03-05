using System.Linq;
using Quantium.Recruitment.Entities;
using System.Collections.Generic;
using AspNetCoreSpa.Server.Repositories;
using AspNetCoreSpa.Server;

namespace Quantium.Recruitment.Infrastructure.Repositories
{
    //public interface IJobLabelDifficultyRepository : IGenericRepository<Job_Difficulty_Label>
    //{
    //    Job_Difficulty_Label FindById(long Id);
    //    void Update(Job_Difficulty_Label entity);
    //    IQueryable<Job_Difficulty_Label> FindByJobId(long Id);
    //}

    public class JobLabelDifficultyRepository : EntityBaseRepository<Job_Difficulty_Label>
    {
        public JobLabelDifficultyRepository(ApplicationDbContext context) : base(context)
        {
        }
        //private readonly IRecruitmentContext _dbContext;
        //public JobLabelDifficultyRepository(IRecruitmentContext dbContext) : base(dbContext)
        //{
        //    _dbContext = dbContext;
        //}

        //public Job_Difficulty_Label FindById(long Id)
        //{
        //    return _dbContext.JobDifficultyLabels.SingleOrDefault(entity => entity.Id == Id);
        //}

        //public IQueryable<Job_Difficulty_Label> FindByJobId(long Id)
        //{
        //    return _dbContext.JobDifficultyLabels.Where(entity => entity.Job.Id == Id);
        //}

        //public void Update(Job_Difficulty_Label entity)
        //{
        //    _dbContext.JobDifficultyLabels.Add(entity);
        //}
    }
}