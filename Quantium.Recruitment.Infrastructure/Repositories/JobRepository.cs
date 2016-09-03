using System.Linq;
using Quantium.Recruitment.Entities;

namespace Quantium.Recruitment.Infrastructure.Repositories
{
    public interface IJobRepository : IGenericRepository<Job>
    {
        Job FindById(long Id);
        void Update(Job entity);
    }

    public class JobRepository : GenericRepository<Job>, IJobRepository
    {
        private readonly IRecruitmentContext _dbContext;
        public JobRepository(IRecruitmentContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public Job FindById(long Id)
        {
            return _dbContext.Jobs.Single(entity => entity.Id == Id);
        }

        public void Update(Job entity)
        {
            _dbContext.Jobs.Add(entity);
        }
    }
}