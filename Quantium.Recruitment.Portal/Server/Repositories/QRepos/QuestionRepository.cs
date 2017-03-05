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

    public class QuestionRepository : EntityBaseRepository<Question>
    {
        public QuestionRepository(ApplicationDbContext context) : base(context)
        {
        }
        //private readonly IRecruitmentContext _dbContext;

        //public QuestionRepository(IRecruitmentContext dbContext) : base(dbContext)
        //{
        //    _dbContext = dbContext;
        //}

        //public Question FindById(long Id)
        //{
        //    return _dbContext.Questions.SingleOrDefault(entity => entity.Id == Id);
        //}

        //public void Update(Question entity)
        //{
        //    //_dbContext.Questions.Attach(entity);
        //    //_dbContext.Entry(entity).State = EntityState.Modified;

        //    _dbContext.SaveChanges();
        //}
    }
}