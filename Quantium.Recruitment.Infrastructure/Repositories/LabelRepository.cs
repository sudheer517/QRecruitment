using System.Linq;
using Quantium.Recruitment.Entities;

namespace Quantium.Recruitment.Infrastructure.Repositories
{
    public interface ILabelRepository : IGenericRepository<Label>
    {
        Label FindById(long Id);
        void Update(Label entity);
        Label FindByName(string name);
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
            return _dbContext.Labels.SingleOrDefault(entity => entity.Id == Id);
        }

        public Label FindByName(string name)
        {
            return _dbContext.Labels.SingleOrDefault(entity => entity.Name == name);
        }

        public void Update(Label entity)
        {
            _dbContext.Labels.Add(entity);
        }
    }
}