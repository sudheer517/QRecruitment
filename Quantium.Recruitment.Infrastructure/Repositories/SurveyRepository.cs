using Quantium.Recruitment.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quantium.Recruitment.Infrastructure.Repositories
{
    public interface ISurveyRepository : IGenericRepository<Survey>
    {
        Survey FindById(long id);
        Survey FindByCandidateId(long candidateId);
        Survey FindByCandidateEmail(string candidateEmail);
        Survey FindActiveSurveyByCandidateEmail(string candidateEmail);
        void Update(Survey entity);

    }
    public class SurveyRepository : GenericRepository<Survey>, ISurveyRepository
    {
        private readonly IRecruitmentContext _dbContext;
        public SurveyRepository(IRecruitmentContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public Survey FindById(long Id)
        {
            return _dbContext.Surveys.SingleOrDefault(entity => entity.Id == Id);
        }
       
        public Survey FindByCandidateId(long candidateId)
        {
            return _dbContext.Surveys.SingleOrDefault(entity => entity.Candidate.Id == candidateId);
        }

        public Survey FindByCandidateEmail(string candidateEmail)
        {
            return _dbContext.Surveys.FirstOrDefault(entity => entity.Candidate.Email == candidateEmail);
        }

        public Survey FindActiveSurveyByCandidateEmail(string candidateEmail)
        {
            return _dbContext.Surveys.FirstOrDefault(entity => entity.Candidate.Email == candidateEmail && entity.IsFinished != true);
        }

        public void Update(Survey entity)
        {
            _dbContext.Surveys.Add(entity);
            _dbContext.Entry(entity).State = System.Data.Entity.EntityState.Modified;
            _dbContext.SaveChanges();
        }
    }
}
