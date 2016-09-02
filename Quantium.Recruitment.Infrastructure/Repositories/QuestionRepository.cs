using System.Collections.Generic;
using System.Linq;
using Quantium.Recruitment.Entities;

namespace Quantium.Recruitment.Infrastructure.Repositories
{
    public interface IQuestionRepository : IRepository<Question>
    {
    }

    public class QuestionRepository : IQuestionRepository
    {
        private readonly IRecruitmentContext _dbContext;
        public QuestionRepository(IRecruitmentContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(Question entity)
        {
            _dbContext.Questions.Add(entity);
        }

        public void Delete(Question entity)
        {
            _dbContext.Questions.Remove(entity);
        }

        public Question FindById(long Id)
        {
            return _dbContext.Questions.Single(entity => entity.Id == Id);
        }

        public IEnumerable<Question> GetAll()
        {
            return _dbContext.Questions;
        }

        public void Update(Question entity)
        {
            _dbContext.Questions.Add(entity);
        }
    }
}