using System.Linq;
using Quantium.Recruitment.Entities;

namespace Quantium.Recruitment.Infrastructure.Repositories
{
    public interface IJobLabelDifficultyRepository : IGenericRepository<Job_Difficulty_Label>
    {
        Job_Difficulty_Label FindById(long Id);
        void Update(Job_Difficulty_Label entity);
    }

    public class JobLabelDifficultyRepository : GenericRepository<Job_Difficulty_Label>, IJobLabelDifficultyRepository
    {
        private readonly IRecruitmentContext _dbContext;
        public JobLabelDifficultyRepository(IRecruitmentContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public Job_Difficulty_Label FindById(long Id)
        {
            return _dbContext.JobDifficultyLabels.SingleOrDefault(entity => entity.Id == Id);
        }

        public void Update(Job_Difficulty_Label entity)
        {
            _dbContext.JobDifficultyLabels.Add(entity);
        }
    }
}