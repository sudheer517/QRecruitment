using System;
using System.Linq;

namespace Quantium.Recruitment.Infrastructure.Repositories
{
    //public abstract class GenericRepository<T> : IGenericRepository<T> where T : class, IDisposable
    //{
    //    private readonly IRecruitmentContext _dbContext;

    //    public GenericRepository(IRecruitmentContext dbContext)
    //    {
    //        _dbContext = dbContext;
    //    }

    //    public T Add(T entity)
    //    {
    //        _dbContext.Entry(entity);
    //        var result = _dbContext.GetSet(typeof(T)).Add(entity);
    //        _dbContext.SaveChanges();

    //        return (T)result;
    //    }

    //    public void Delete(T entity)
    //    {
    //        _dbContext.GetSet(typeof(T)).Remove(entity);
    //        _dbContext.SaveChanges();
    //    }

    //    public IQueryable<T> GetAll()
    //    {
    //        var dbSet = _dbContext.GetSet(typeof(T));
    //        return dbSet.Cast<T>();
    //    }

    //    public void Dispose()
    //    {
    //        if (_dbContext != null)
    //            _dbContext.Dispose();
    //    }
    //}
}
