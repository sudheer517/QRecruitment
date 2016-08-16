using System.Linq;
using Quantium.Recruitment.Infrastructure;
using Quantium.Recruitment.Services.Models;

namespace Quantium.Recruitment.Services.Repositories
{
    public interface IQuestionRepository
    {
        void AddQuestion(QuestionDto questionModel);
        QuestionDto GetQestion(long questionId);
    }

    public class QuestionRepository : IQuestionRepository
    {
        private readonly IRecruitmentContext _dbContext;

        public QuestionRepository(IRecruitmentContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddQuestion(QuestionDto question)
        {
            _dbContext.Questions.Add(question);
            _dbContext.SaveChanges();
        }

        public QuestionDto GetQestion(long questionId)
        {
            return (QuestionDto)_dbContext.Questions.Single(q => q.Id == questionId);
        }
    }
}
