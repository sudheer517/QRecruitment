using System.Linq;
using Quantium.Recruitment.Entities;

namespace Quantium.Recruitment.Infrastructure.Repositories
{
    public interface IQuestionRepository
    {
        void AddQuestion(Question questionModel);
        Question GetQestion(long questionId);
    }

    public class QuestionRepository : IQuestionRepository
    {
        private readonly IRecruitmentContext _dbContext;

        public QuestionRepository(IRecruitmentContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddQuestion(Question question)
        {
            _dbContext.Questions.Add(question);
            _dbContext.SaveChanges();
        }

        public Question GetQestion(long questionId)
        {
            return _dbContext.Questions.Single(q => q.Id == questionId);
        }
    }
}
