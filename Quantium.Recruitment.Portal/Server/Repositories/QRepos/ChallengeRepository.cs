using System.Linq;
using Quantium.Recruitment.Entities;
using AspNetCoreSpa.Server;
using AspNetCoreSpa.Server.Repositories;

namespace Quantium.Recruitment.Infrastructure.Repositories
{
    //public interface IChallengeRepository : IGenericRepository<Challenge>
    //{
    //    Challenge FindById(long Id);
    //    void Update(Challenge entity);
    //    Challenge FindByIdUsingNewContext(long id);
    //    void UpdateWithNewContext(Challenge entity, IRecruitmentContext context);
    //}

    public class ChallengeRepository : EntityBaseRepository<Challenge>
    {
        public ChallengeRepository(ApplicationDbContext context) : base(context)
        {
        }

        //private readonly IRecruitmentContext _dbContext;
        ////private readonly IResolver<RecruitmentContext> _resolver;
        //private readonly IConnectionString _connString; 
        //public ChallengeRepository(IRecruitmentContext dbContext, IConnectionString connString) : base(dbContext)
        //{
        //    _dbContext = dbContext;
        //    _connString = connString;
        //    //_resolver = resolver;
        //}

        //public Challenge FindById(long Id)
        //{
        //    return _dbContext.Challenges.Single(entity => entity.Id == Id);
        //}

        //public Challenge FindByIdUsingNewContext(long Id)
        //{
        //    //var _newContext = _resolver.Resolve();

        //    using (var _newContext = new RecruitmentContext(_connString))
        //    {
        //        return _newContext.Challenges.Single(entity => entity.Id == Id);
        //    }

        //}

        //public void Update(Challenge entity)
        //{
        //    _dbContext.Challenges.Attach(entity);
        //    _dbContext.Entry(entity).State = System.Data.Entity.EntityState.Modified;

        //    foreach (var candidateSelectedOption in entity.CandidateSelectedOptions)
        //    {
        //        _dbContext.Entry(candidateSelectedOption).State = System.Data.Entity.EntityState.Added;
        //    }

        //    _dbContext.SaveChanges();
        //}

        //public void UpdateWithNewContext(Challenge entity, IRecruitmentContext context)
        //{
        //    context.Challenges.Attach(entity);
        //    context.Entry(entity).State = System.Data.Entity.EntityState.Modified;

        //    foreach (var candidateSelectedOption in entity.CandidateSelectedOptions)
        //    {
        //        context.Entry(candidateSelectedOption).State = System.Data.Entity.EntityState.Added;
        //    }

        //    context.SaveChanges();
        //}
    }
}