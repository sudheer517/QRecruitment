using System.Linq;
using Quantium.Recruitment.Entities;
using System.Collections.Generic;
using AspNetCoreSpa.Server.Repositories;
using AspNetCoreSpa.Server;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System;

namespace Quantium.Recruitment.Infrastructure.Repositories
{
    //public interface IJobLabelDifficultyRepository : IGenericRepository<Job_Difficulty_Label>
    //{
    //    Job_Difficulty_Label FindById(long Id);
    //    void Update(Job_Difficulty_Label entity);
    //    IQueryable<Job_Difficulty_Label> FindByJobId(long Id);
    //}
    public class JobDifficultyLabelRepository : EntityBaseRepository<Job_Difficulty_Label>
    {
        private ApplicationDbContext _context;

        public JobDifficultyLabelRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
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

        public override IEnumerable<Job_Difficulty_Label> FindBy(Expression<Func<Job_Difficulty_Label, bool>> predicate)
        {
            IQueryable<Job_Difficulty_Label> query = _context.Set<Job_Difficulty_Label>().Where(predicate);

            query = query.Include(jdl => jdl.Difficulty).Include(jdl => jdl.Label);

            return query.AsEnumerable();
        }

        

        //public void Update(Job_Difficulty_Label entity)
        //{
        //    _dbContext.JobDifficultyLabels.Add(entity);
        //}
    }
}