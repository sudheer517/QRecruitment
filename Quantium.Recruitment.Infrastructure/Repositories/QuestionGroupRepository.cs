using System.Linq;
using Quantium.Recruitment.Entities;

namespace Quantium.Recruitment.Infrastructure.Repositories
{
    public interface IQuestionGroupRepository : IGenericRepository<QuestionGroup>
    {
        QuestionGroup FindById(long Id);
        void Update(QuestionGroup entity);
    }

    public class QuestionGroupRepository : GenericRepository<QuestionGroup>, IQuestionGroupRepository
    {
        private readonly IRecruitmentContext _dbContext;
        public QuestionGroupRepository(IRecruitmentContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public QuestionGroup FindById(long Id)
        {
            return _dbContext.QuestionGroups.Single(entity => entity.Id == Id);
        }

        public void Update(QuestionGroup entity)
        {
            _dbContext.QuestionGroups.Add(entity);
        }
    }
}