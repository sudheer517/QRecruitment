using System.Collections.Generic;
using System.Linq;
using Quantium.Recruitment.Entities;

namespace Quantium.Recruitment.Infrastructure.Repositories
{
    public interface IOptionRepository : IRepository<Option>
    {
    }

    public class OptionRepository : IOptionRepository
    {
        private readonly IRecruitmentContext _dbContext;
        public OptionRepository(IRecruitmentContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(Option entity)
        {
            _dbContext.Options.Add(entity);
        }

        public void Delete(Option entity)
        {
            _dbContext.Options.Remove(entity);
        }

        public Option FindById(long Id)
        {
            return _dbContext.Options.Single(entity => entity.Id == Id);
        }

        public IEnumerable<Option> GetAll()
        {
            return _dbContext.Options;
        }

        public void Update(Option entity)
        {
            _dbContext.Options.Add(entity);
        }
    }
}