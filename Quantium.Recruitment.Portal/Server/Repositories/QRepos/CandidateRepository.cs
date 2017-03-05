using System.Linq;
using Quantium.Recruitment.Entities;
using AspNetCoreSpa.Server;
using AspNetCoreSpa.Server.Repositories;
using AspNetCoreSpa.Server.Repositories.Abstract;

namespace Quantium.Recruitment.Infrastructure.Repositories
{

    public class CandidateRepository : EntityBaseRepository<Candidate>
    {
        public CandidateRepository(ApplicationDbContext context) : base(context)
        {
        }

        //public Candidate FindById(long Id)
        //{
        //    return _dbContext.Candidates.SingleOrDefault(entity => entity.Id == Id);
        //}

        //public Candidate FindByEmail(string email)
        //{
        //    return _dbContext.Candidates.SingleOrDefault(entity => entity.Email == email);
        //}

        //public void Update(Candidate entity)
        //{
        //    _dbContext.Candidates.Add(entity);
        //    _dbContext.Entry(entity).State = System.Data.Entity.EntityState.Modified;
        //    _dbContext.SaveChanges();
        //}
    }
}