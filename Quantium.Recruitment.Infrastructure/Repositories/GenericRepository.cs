using System;
using System.Linq;

namespace Quantium.Recruitment.Infrastructure.Repositories
{
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly IRecruitmentContext _dbContext;

        public GenericRepository(IRecruitmentContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(T entity)
        {
            _dbContext.GetSet(typeof(T)).Add(entity);
            _dbContext.SaveChanges();
        }

        public void Delete(T entity)
        {
            _dbContext.GetSet(typeof(T)).Remove(entity);
            _dbContext.SaveChanges();
        }

        public IQueryable<T> GetAll()
        {
            var dbSet = _dbContext.GetSet(typeof(T));
            return dbSet.Cast<T>();
        }
    }
}
