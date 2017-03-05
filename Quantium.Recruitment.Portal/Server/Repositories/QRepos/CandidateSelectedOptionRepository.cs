using System.Linq;
using Quantium.Recruitment.Entities;
using AspNetCoreSpa.Server.Repositories;
using AspNetCoreSpa.Server;

namespace Quantium.Recruitment.Infrastructure.Repositories
{
    //public interface ICandidateSelectedOptionRepository : IGenericRepository<CandidateSelectedOption>
    //{
    //    CandidateSelectedOption FindById(long Id);
    //    void Update(CandidateSelectedOption entity);
    //}

    public class CandidateSelectedOptionRepository : EntityBaseRepository<CandidateSelectedOption>
    {
        public CandidateSelectedOptionRepository(ApplicationDbContext context) : base(context)
        {
        }
        //private readonly IRecruitmentContext _dbContext;
        //public CandidateSelectedOptionRepository(IRecruitmentContext dbContext) : base(dbContext)
        //{
        //    _dbContext = dbContext;
        //}

        //public CandidateSelectedOption FindById(long Id)
        //{
        //    return _dbContext.CandidateSelectedOptions.Single(entity => entity.Id == Id);
        //}

        //public void Update(CandidateSelectedOption entity)
        //{
        //    _dbContext.CandidateSelectedOptions.Add(entity);
        //}
    }
}