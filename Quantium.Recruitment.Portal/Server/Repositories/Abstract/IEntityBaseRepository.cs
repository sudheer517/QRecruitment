using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AspNetCoreSpa.Server.Entities;
using System.Linq;

namespace AspNetCoreSpa.Server.Repositories.Abstract
{
    public interface IEntityBaseRepository<T> where T : class, IEntityBase, new()
    {
        IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties);
        Task<IEnumerable<T>> AllIncludingAsync(params Expression<Func<T, object>>[] includeProperties);
        IEnumerable<T> GetAll();
        Task<IEnumerable<T>> GetAllAsync();
        T GetSingle(long id);
        T GetSingle(Expression<Func<T, bool>> predicate);
        T GetSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
        Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate);
        Task<T> GetSingleAsyncIncluding(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
        IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate);
        Task<IList<T>> FindByIncludeAllAsync(Expression<Func<T, bool>> predicate);
        Task<IList<T>> FindByAsync(Expression<Func<T, bool>> predicate);
        T Add(T entity);
        void Delete(T entity);
        void Edit(T entity);
        void CommitAsync();
        void Commit();
        T GetSingleUsingNewContext(long id);
        T UpdateWithNewContext(T entity);
        void Update(T entity);
    }
}
