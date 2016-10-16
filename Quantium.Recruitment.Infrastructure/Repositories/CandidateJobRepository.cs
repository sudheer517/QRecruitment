using System.Linq;
using Quantium.Recruitment.Entities;

namespace Quantium.Recruitment.Infrastructure.Repositories
{
    public interface ICandidateJobRepository : IGenericRepository<Candidate_Job>
    {
        Candidate_Job FindById(long Id);
        void Update(Candidate_Job entity);
    }

    public class CandidateJobRepository : GenericRepository<Candidate_Job>, ICandidateJobRepository
    {
        private readonly IRecruitmentContext _dbContext;
        public CandidateJobRepository(IRecruitmentContext dbContext): base(dbContext)
        {
            _dbContext = dbContext;
        }

        public Candidate_Job FindById(long Id)
        {
            return _dbContext.CandidateJobs.Single(entity => entity.Id == Id);
        }

        public void Update(Candidate_Job entity)
        {
            _dbContext.CandidateJobs.Add(entity);
        }
    }
}