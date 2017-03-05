using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quantium.Recruitment.Entities;
using AspNetCoreSpa.Server.Repositories;
using AspNetCoreSpa.Server;

namespace Quantium.Recruitment.Infrastructure.Repositories
{
    //public interface ISurveyChallengeRepository : IGenericRepository<SurveyChallenge>
    //{
    //    SurveyChallenge FindById(long Id);
    //    void Update(SurveyChallenge entity);        
    //}

    public class SurveyChallengeRepository : EntityBaseRepository<SurveyChallenge>
    {
        public SurveyChallengeRepository(ApplicationDbContext context) : base(context)
        {
        }
        //private readonly IRecruitmentContext _dbContext;
        //public SurveyChallengeRepository(IRecruitmentContext dbContext) : base(dbContext)
        //{
        //    _dbContext = dbContext;
        //}

        //public SurveyChallenge FindById(long Id)
        //{
        //    return _dbContext.SurveyChallenges.Single(entity => entity.Id == Id);
        //}

        //public void Update(SurveyChallenge entity)
        //{
        //    _dbContext.SurveyChallenges.Attach(entity);
        //    _dbContext.Entry(entity).State = System.Data.Entity.EntityState.Modified;
        //    _dbContext.SaveChanges();
        //}
    }
}
