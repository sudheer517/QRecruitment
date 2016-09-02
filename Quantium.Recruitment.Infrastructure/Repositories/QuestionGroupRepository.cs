using System.Collections.Generic;
using System.Linq;
using Quantium.Recruitment.Entities;

namespace Quantium.Recruitment.Infrastructure.Repositories
{
    public interface IQuestionGroupRepository : IRepository<QuestionGroup>
    {
    }

    public class QuestionGroupRepository : IQuestionGroupRepository
    {
        private readonly IRecruitmentContext _dbContext;
        public QuestionGroupRepository(IRecruitmentContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(QuestionGroup entity)
        {
            _dbContext.QuestionGroups.Add(entity);
        }

        public void Delete(QuestionGroup entity)
        {
            _dbContext.QuestionGroups.Remove(entity);
        }

        public QuestionGroup FindById(long Id)
        {
            return _dbContext.QuestionGroups.Single(entity => entity.Id == Id);
        }

        public IEnumerable<QuestionGroup> GetAll()
        {
            return _dbContext.QuestionGroups;
        }

        public void Update(QuestionGroup entity)
        {
            _dbContext.QuestionGroups.Add(entity);
        }
    }
}