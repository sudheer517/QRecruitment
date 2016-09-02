using System.Collections.Generic;
using System.Linq;
using Quantium.Recruitment.Entities;

namespace Quantium.Recruitment.Infrastructure.Repositories
{
    public interface ILabelRepository : IRepository<Label>
    {
    }

    public class LabelRepository : ILabelRepository
    {
        private readonly IRecruitmentContext _dbContext;
        public LabelRepository(IRecruitmentContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(Label entity)
        {
            _dbContext.Labels.Add(entity);
        }

        public void Delete(Label entity)
        {
            _dbContext.Labels.Remove(entity);
        }

        public Label FindById(long Id)
        {
            return _dbContext.Labels.Single(entity => entity.Id == Id);
        }

        public IEnumerable<Label> GetAll()
        {
            return _dbContext.Labels;
        }

        public void Update(Label entity)
        {
            _dbContext.Labels.Add(entity);
        }
    }
}