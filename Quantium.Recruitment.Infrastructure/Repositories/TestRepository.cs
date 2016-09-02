using System.Collections.Generic;
using System.Linq;
using Quantium.Recruitment.Entities;

namespace Quantium.Recruitment.Infrastructure.Repositories
{
    public interface ITestRepository : IRepository<Test>
    {
    }

    public abstract class TestRepository : ITestRepository
    {
        private readonly IRecruitmentContext _dbContext;
        public TestRepository(IRecruitmentContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(Test entity)
        {
            _dbContext.Tests.Add(entity);
        }

        public void Delete(Test entity)
        {
            _dbContext.Tests.Remove(entity);
        }

        public Test FindById(long Id)
        {
            return _dbContext.Tests.Single(entity => entity.Id == Id);
        }

        public IEnumerable<Test> GetAll()
        {
            return _dbContext.Tests;
        }

        public void Update(Test entity)
        {
            _dbContext.Tests.Add(entity);
        }
    }
}