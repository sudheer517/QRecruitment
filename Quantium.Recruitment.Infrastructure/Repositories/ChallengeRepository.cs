using System.Linq;
using Quantium.Recruitment.Entities;

namespace Quantium.Recruitment.Infrastructure.Repositories
{
    public interface IChallengeRepository : IGenericRepository<Challenge>
    {
        Challenge FindById(long Id);
        void Update(Challenge entity);
    }

    public class ChallengeRepository : GenericRepository<Challenge>, IChallengeRepository
    {
        private readonly IRecruitmentContext _dbContext;
        public ChallengeRepository(IRecruitmentContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public Challenge FindById(long Id)
        {
            return _dbContext.Challenges.Single(entity => entity.Id == Id);
        }

        public void Update(Challenge entity)
        {
            _dbContext.Challenges.Attach(entity);
            _dbContext.Entry(entity).State = System.Data.Entity.EntityState.Modified;

            foreach (var candidateSelectedOption in entity.CandidateSelectedOptions)
            {
                _dbContext.Entry(candidateSelectedOption).State = System.Data.Entity.EntityState.Added;
            }

            _dbContext.SaveChanges();
        }
    }
}