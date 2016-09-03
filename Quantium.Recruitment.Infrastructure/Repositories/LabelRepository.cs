using System.Linq;
using Quantium.Recruitment.Entities;

namespace Quantium.Recruitment.Infrastructure.Repositories
{
    public interface ILabelRepository : IGenericRepository<Label>
    {
        Label FindById(long Id);
        void Update(Label entity);
    }

    public class LabelRepository : GenericRepository<Label>, ILabelRepository
    {
        private readonly IRecruitmentContext _dbContext;
        public LabelRepository(IRecruitmentContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public Label FindById(long Id)
        {
            return _dbContext.Labels.Single(entity => entity.Id == Id);
        }

        public void Update(Label entity)
        {
            _dbContext.Labels.Add(entity);
        }
    }
}