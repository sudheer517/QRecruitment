using System.Linq;
using Quantium.Recruitment.Entities;

namespace Quantium.Recruitment.Infrastructure.Repositories
{
    public interface IJobLabelDifficultyRepository : IGenericRepository<Job_Label_Difficulty>
    {
        Job_Label_Difficulty FindById(long Id);
        void Update(Job_Label_Difficulty entity);
    }

    public class JobLabelDifficultyRepository : GenericRepository<Job_Label_Difficulty>, IJobLabelDifficultyRepository
    {
        private readonly IRecruitmentContext _dbContext;
        public JobLabelDifficultyRepository(IRecruitmentContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public Job_Label_Difficulty FindById(long Id)
        {
            return _dbContext.JobLabelDifficulties.SingleOrDefault(entity => entity.Id == Id);
        }

        public void Update(Job_Label_Difficulty entity)
        {
            _dbContext.JobLabelDifficulties.Add(entity);
        }
    }
}