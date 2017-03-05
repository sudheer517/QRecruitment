using System.Linq;
using Quantium.Recruitment.Entities;
using AspNetCoreSpa.Server.Repositories;
using AspNetCoreSpa.Server;

namespace Quantium.Recruitment.Infrastructure.Repositories
{
    //public interface ISurveyQuestionRepository: IGenericRepository<SurveyQuestion>
    //{
    //    SurveyQuestion FindById(long Id);
    //    void Update(SurveyQuestion entity);
    //}

    public class SurveyQuestionRepository : EntityBaseRepository<SurveyQuestion>
    {
        public SurveyQuestionRepository(ApplicationDbContext context) : base(context)
        {
        }
        //private readonly IRecruitmentContext _dbContext;
        //public SurveyQuestionRepository(IRecruitmentContext dbContext) : base(dbContext)
        //{
        //    _dbContext = dbContext;
        //}

        //public SurveyQuestion FindById(long Id)
        //{
        //    return _dbContext.SurveyQuestions.SingleOrDefault(entity => entity.Id == Id);
        //}

        //public void Update(SurveyQuestion entity)
        //{
        //    _dbContext.SaveChanges();
        //}
    }
}
