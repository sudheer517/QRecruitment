using System.Collections.Generic;
using System.Linq;
using Quantium.Recruitment.Entities;

namespace Quantium.Recruitment.Infrastructure.Repositories
{
    public interface IChallengeRepository : IRepository<Challenge>
    {
    }

    public class ChallengeRepository : IChallengeRepository
    {
        private readonly IRecruitmentContext _dbContext;
        public ChallengeRepository(IRecruitmentContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(Challenge entity)
        {
            _dbContext.Challenges.Add(entity);
        }

        public void Delete(Challenge entity)
        {
            _dbContext.Challenges.Remove(entity);
        }

        public Challenge FindById(long Id)
        {
            return _dbContext.Challenges.Single(entity => entity.Id == Id);
        }

        public IEnumerable<Challenge> GetAll()
        {
            return _dbContext.Challenges;
        }

        public void Update(Challenge entity)
        {
            _dbContext.Challenges.Add(entity);
        }
    }
}