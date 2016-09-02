using System.Collections.Generic;
using System.Linq;
using Quantium.Recruitment.Entities;

namespace Quantium.Recruitment.Infrastructure.Repositories
{
    public interface IJobRepository : IRepository<Job>
    {
    }

    public class JobRepository : IJobRepository
    {
        private readonly IRecruitmentContext _dbContext;
        public JobRepository(IRecruitmentContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(Job entity)
        {
            _dbContext.Jobs.Add(entity);
        }

        public void Delete(Job entity)
        {
            _dbContext.Jobs.Remove(entity);
        }

        public Job FindById(long Id)
        {
            return _dbContext.Jobs.Single(entity => entity.Id == Id);
        }

        public IEnumerable<Job> GetAll()
        {
            return _dbContext.Jobs;
        }

        public void Update(Job entity)
        {
            _dbContext.Jobs.Add(entity);
        }
    }
}