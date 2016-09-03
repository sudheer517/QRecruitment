using System.Linq;
using Quantium.Recruitment.Entities;

namespace Quantium.Recruitment.Infrastructure.Repositories
{
    public interface ICandidateRepository : IGenericRepository<Candidate>
    {
        Candidate FindById(long Id);
        void Update(Candidate entity);
    }

    public class CandidateRepository : GenericRepository<Candidate>, ICandidateRepository
    {
        private readonly IRecruitmentContext _dbContext;
        public CandidateRepository(IRecruitmentContext dbContext): base(dbContext)
        {
            _dbContext = dbContext;
        }

        public Candidate FindById(long Id)
        {
            return _dbContext.Candidates.Single(entity => entity.Id == Id);
        }

        public void Update(Candidate entity)
        {
            _dbContext.Candidates.Add(entity);
        }
    }
}