using System.Linq;
using Quantium.Recruitment.Entities;
using AspNetCoreSpa.Server;
using AspNetCoreSpa.Server.Repositories;
using System;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;

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
        private readonly IServiceProvider _serviceProvider;
        private readonly ApplicationDbContext _context;
        public ChallengeRepository(ApplicationDbContext context, IServiceProvider serviceProvider) : base(context)
        {
            _context = context;
            _serviceProvider = serviceProvider;
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
        public override Challenge GetSingleUsingNewContext(long id)
        {
            var newContext = (ApplicationDbContext)_serviceProvider.GetService(typeof(ApplicationDbContext));
            return newContext.Challenges.Single(c => c.Id == id);
        }

        public override Challenge UpdateWithNewContext(Challenge entity)
        {
            var newContext = (ApplicationDbContext)_serviceProvider.GetService(typeof(ApplicationDbContext));
            EntityEntry dbEntityEntry = newContext.Entry<Challenge>(entity);
            dbEntityEntry.State = EntityState.Modified;
            newContext.SaveChanges();
            return entity;
        }

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