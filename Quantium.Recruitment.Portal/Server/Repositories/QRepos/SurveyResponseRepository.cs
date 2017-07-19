using System.Collections.Generic;
using System.Linq;
using Quantium.Recruitment.Entities;
using AspNetCoreSpa.Server.Repositories;
using AspNetCoreSpa.Server;

namespace Quantium.Recruitment.Infrastructure.Repositories
{
    //public interface IQuestionRepository : IGenericRepository<Question>
    //{
    //    Question FindById(long Id);
    //    void Update(Question entity);
    //}

    public class SurveyResponseRepository : EntityBaseRepository<SurveyResponse>
    {
        public SurveyResponseRepository(ApplicationDbContext context) : base(context)
        {
        }
        //private readonly IRecruitmentContext _dbContext;
        
    }

    public class SurveyAdminCommentsRepository : EntityBaseRepository<SurveyAdminComments>
    {
        public SurveyAdminCommentsRepository(ApplicationDbContext context) : base(context)
        {
        }
        //private readonly IRecruitmentContext _dbContext;
       
    }
}