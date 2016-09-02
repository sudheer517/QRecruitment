using System.Collections.Generic;
using System.Linq;
using Quantium.Recruitment.Entities;

namespace Quantium.Recruitment.Infrastructure.Repositories
{
    public interface ICandidateSelectedOptionRepository : IRepository<CandidateSelectedOption>
    {
    }

    public class CandidateSelectedOptionRepository : ICandidateSelectedOptionRepository
    {
        private readonly IRecruitmentContext _dbContext;
        public CandidateSelectedOptionRepository(IRecruitmentContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(CandidateSelectedOption entity)
        {
            _dbContext.CandidateSelectedOptions.Add(entity);
        }

        public void Delete(CandidateSelectedOption entity)
        {
            _dbContext.CandidateSelectedOptions.Remove(entity);
        }

        public CandidateSelectedOption FindById(long Id)
        {
            return _dbContext.CandidateSelectedOptions.Single(entity => entity.Id == Id);
        }

        public IEnumerable<CandidateSelectedOption> GetAll()
        {
            return _dbContext.CandidateSelectedOptions;
        }

        public void Update(CandidateSelectedOption entity)
        {
            _dbContext.CandidateSelectedOptions.Add(entity);
        }
    }
}