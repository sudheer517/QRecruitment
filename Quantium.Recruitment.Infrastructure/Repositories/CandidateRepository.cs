using System.Linq;
using Quantium.Recruitment.Entities;

namespace Quantium.Recruitment.Infrastructure.Repositories
{
    public interface ICandidateRepository : IGenericRepository<Candidate>
    {
        Candidate FindById(long Id);
        Candidate FindByEmail(string email);
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
            return _dbContext.Candidates.SingleOrDefault(entity => entity.Id == Id);
        }

        public Candidate FindByEmail(string email)
        {
            return _dbContext.Candidates.SingleOrDefault(entity => entity.Email == email);
        }

        public void Update(Candidate entity)
        {
            _dbContext.Candidates.Add(entity);
            _dbContext.Entry(entity).State = System.Data.Entity.EntityState.Modified;
            _dbContext.SaveChanges();
        }
    }
}