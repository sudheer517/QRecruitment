using System.Collections.Generic;
using System.Linq;
using Quantium.Recruitment.Entities;

namespace Quantium.Recruitment.Infrastructure.Repositories
{
    public interface IQuestionRepository : IGenericRepository<Question>
    {
        Question FindById(long Id);
        void Update(Question entity);
    }

    public class QuestionRepository :GenericRepository<Question>,  IQuestionRepository
    {
        private readonly IRecruitmentContext _dbContext;

        public QuestionRepository(IRecruitmentContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public Question FindById(long Id)
        {
            return _dbContext.Questions.Single(entity => entity.Id == Id);
        }

        public void Update(Question entity)
        {
            _dbContext.Questions.Add(entity);
        }
    }
}