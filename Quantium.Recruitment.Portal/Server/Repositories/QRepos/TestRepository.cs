using System.Linq;
using Quantium.Recruitment.Entities;
using AspNetCoreSpa.Server.Repositories;
using AspNetCoreSpa.Server;

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
        public TestRepository(ApplicationDbContext context) : base(context)
        {
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

        //public Test FindByCandidateEmail(string candidateEmail)
        //{
        //    return _dbContext.Tests.FirstOrDefault(entity => entity.Candidate.Email == candidateEmail);
        //}

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