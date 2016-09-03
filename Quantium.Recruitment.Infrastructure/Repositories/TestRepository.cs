using System.Linq;
using Quantium.Recruitment.Entities;

namespace Quantium.Recruitment.Infrastructure.Repositories
{
    public interface ITestRepository : IGenericRepository<Test>
    {
        Test FindById(long Id);
        void Update(Test entity);
    }

    public class TestRepository : GenericRepository<Test>, ITestRepository
    {
        private readonly IRecruitmentContext _dbContext;
        public TestRepository(IRecruitmentContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public Test FindById(long Id)
        {
            return _dbContext.Tests.Single(entity => entity.Id == Id);
        }

        public void Update(Test entity)
        {
            _dbContext.Tests.Add(entity);
        }
    }
}