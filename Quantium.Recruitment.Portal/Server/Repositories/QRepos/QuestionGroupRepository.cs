using System.Linq;
using Quantium.Recruitment.Entities;
using AspNetCoreSpa.Server;
using AspNetCoreSpa.Server.Repositories;

namespace Quantium.Recruitment.Infrastructure.Repositories
{
    //public interface IQuestionGroupRepository : IGenericRepository<QuestionGroup>
    //{
    //    QuestionGroup FindById(long Id);
    //    void Update(QuestionGroup entity);
    //    QuestionGroup FindByName(string name);
    //}

    public class QuestionGroupRepository : EntityBaseRepository<QuestionGroup>
    {
        public QuestionGroupRepository(ApplicationDbContext context) : base(context)
        {
        }

        //private readonly IRecruitmentContext _dbContext;
        //public QuestionGroupRepository(IRecruitmentContext dbContext) : base(dbContext)
        //{
        //    _dbContext = dbContext;
        //}

        //public QuestionGroup FindById(long Id)
        //{
        //    return _dbContext.QuestionGroups.Single(entity => entity.Id == Id);
        //}

        //public QuestionGroup FindByName(string name)
        //{
        //    return _dbContext.QuestionGroups.SingleOrDefault(entity => entity.Description == name);
        //}

        //public void Update(QuestionGroup entity)
        //{
        //    _dbContext.QuestionGroups.Add(entity);
        //}
    }
}