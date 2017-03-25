using System.Linq;
using Quantium.Recruitment.Entities;
using AspNetCoreSpa.Server.Repositories;
using AspNetCoreSpa.Server;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;
using System.Threading.Tasks;

namespace Quantium.Recruitment.Infrastructure.Repositories
{
    //public interface ITestRepository : IGenericRepository<Test>
    //{
    //    Test FindById(long Id);
    //    void Update(Test entity);
    //    Test FindByCandidateId(long candidateId);
    //    Test FindByCandidateEmail(string candidateEmail);
    //    Test FindActiveTestByCandidateEmail(string candidateEmail);
    //}

    public class TestRepository : EntityBaseRepository<Test>
    {
        private readonly ApplicationDbContext _context; 
        public TestRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        //private readonly IRecruitmentContext _dbContext;
        //public TestRepository(IRecruitmentContext dbContext) : base(dbContext)
        //{
        //    _dbContext = dbContext;
        //}

        //public Test FindById(long Id)
        //{
        //    return _dbContext.Tests.SingleOrDefault(entity => entity.Id == Id);
        //}

        //public Test FindByCandidateId(long candidateId)
        //{
        //    return _dbContext.Tests.SingleOrDefault(entity => entity.Candidate.Id == candidateId);
        //}

        public override IEnumerable<Test> FindBy(Expression<Func<Test, bool>> predicate)
        {
            IQueryable<Test> query = _context.Set<Test>().Where(predicate);

            query =
                query.
                Include(t => t.Candidate).
                Include(t => t.Challenges).
                    ThenInclude(c => c.Question).
                        ThenInclude(q => q.Options);

            return query.AsEnumerable();
        }

        public override Test GetSingle(Expression<Func<Test, bool>> predicate)
        {
            IQueryable<Test> query = _context.Set<Test>().Where(predicate);

            query =
                query.
                Include(t => t.Candidate).
                Include(t => t.Job).
                    ThenInclude(j => j.JobDifficultyLabels).
                        ThenInclude(jdl => jdl.Label).
                Include(t => t.Job).
                    ThenInclude(j => j.JobDifficultyLabels).
                        ThenInclude(jdl => jdl.Difficulty).
                Include(t => t.Challenges).
                    ThenInclude(c => c.CandidateSelectedOptions).
                Include(t => t.Challenges).
                        ThenInclude(c => c.Question).
                            ThenInclude(q => q.Options);

            return query.Where(predicate).FirstOrDefault();
        }

        public override async Task<IList<Test>> FindByIncludeAllAsync(Expression<Func<Test, bool>> predicate)
        {
            IQueryable<Test> query = _context.Set<Test>();

            query =
                query.
                Include(t => t.Candidate).
                Include(t => t.Job).
                    ThenInclude(j => j.JobDifficultyLabels).
                        ThenInclude(jdl => jdl.Label).
                Include(t => t.Job).
                    ThenInclude(j => j.JobDifficultyLabels).
                        ThenInclude(jdl => jdl.Difficulty).
                Include(t => t.Challenges).
                    ThenInclude(c => c.CandidateSelectedOptions).
                Include(t => t.Challenges).
                        ThenInclude(c => c.Question).
                            ThenInclude(q => q.Options);

            return await query.Where(predicate).ToListAsync();
        }

        //public Test FindActiveTestByCandidateEmail(string candidateEmail)
        //{
        //    return _dbContext.Tests.FirstOrDefault(entity => entity.Candidate.Email == candidateEmail && entity.IsFinished != true);
        //}

        //public void Update(Test entity)
        //{
        //    _dbContext.Tests.Add(entity);
        //    _dbContext.Entry(entity).State = System.Data.Entity.EntityState.Modified;
        //    _dbContext.SaveChanges();
        //}
    }
}