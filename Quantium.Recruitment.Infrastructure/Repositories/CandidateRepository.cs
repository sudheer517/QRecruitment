using System.Collections.Generic;
using System.Linq;
using Quantium.Recruitment.Entities;

namespace Quantium.Recruitment.Infrastructure.Repositories
{
    public interface ICandidateRepository : IRepository<Candidate>
    {
    }

    public class CandidateRepository : ICandidateRepository
    {
        private readonly IRecruitmentContext _dbContext;
        public CandidateRepository(IRecruitmentContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(Candidate entity)
        {
            _dbContext.Candidates.Add(entity);
        }

        public void Delete(Candidate entity)
        {
            _dbContext.Candidates.Remove(entity);
        }

        public Candidate FindById(long Id)
        {
            return _dbContext.Candidates.Single(entity => entity.Id == Id);
        }

        public IEnumerable<Candidate> GetAll()
        {
            return _dbContext.Candidates;
        }

        public void Update(Candidate entity)
        {
            _dbContext.Candidates.Add(entity);
        }
    }
}