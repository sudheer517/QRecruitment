using System.Linq;
using Quantium.Recruitment.Entities;
using AspNetCoreSpa.Server;
using AspNetCoreSpa.Server.Repositories;

namespace Quantium.Recruitment.Infrastructure.Repositories
{
    public class CandidateJobRepository : EntityBaseRepository<Candidate_Job>
    {
        public CandidateJobRepository(ApplicationDbContext context) : base(context)
        {
        }

        //public Candidate_Job FindById(long Id)
        //{
        //    return _dbContext.CandidateJobs.Single(entity => entity.Id == Id);
        //}

        //public void Update(Candidate_Job entity)
        //{
        //    _dbContext.CandidateJobs.Add(entity);
        //}
    }
}