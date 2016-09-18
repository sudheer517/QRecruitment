using System.Linq;
using Quantium.Recruitment.Entities;

namespace Quantium.Recruitment.Infrastructure.Repositories
{
    public interface IOptionRepository : IGenericRepository<Option>
    {
        Option FindById(long Id);
        void Update(Option entity);
    }

    public class OptionRepository : GenericRepository<Option>, IOptionRepository
    {
        private readonly IRecruitmentContext _dbContext;
        public OptionRepository(IRecruitmentContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public Option FindById(long Id)
        {
            return _dbContext.Options.Single(entity => entity.Id == Id);
        }

        public void Update(Option entity)
        {
            _dbContext.SaveChanges();
        }
    }
}