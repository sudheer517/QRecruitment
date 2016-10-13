using System.Linq;
using Quantium.Recruitment.Entities;

namespace Quantium.Recruitment.Infrastructure.Repositories
{
    public interface IQuestionLabelDifficultyRepository : IGenericRepository<Question_Label_Difficulty>
    {
        Question_Label_Difficulty FindById(long Id);
        void Update(Question_Label_Difficulty entity);
    }

    public class QuestionLabelDifficultyRepository : GenericRepository<Question_Label_Difficulty>, IQuestionLabelDifficultyRepository
    {
        private readonly IRecruitmentContext _dbContext;
        public QuestionLabelDifficultyRepository(IRecruitmentContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public Question_Label_Difficulty FindById(long Id)
        {
            return _dbContext.QuestionLabelDifficulties.SingleOrDefault(entity => entity.Id == Id);
        }

        public void Update(Question_Label_Difficulty entity)
        {
            _dbContext.QuestionLabelDifficulties.Add(entity);
        }
    }
}