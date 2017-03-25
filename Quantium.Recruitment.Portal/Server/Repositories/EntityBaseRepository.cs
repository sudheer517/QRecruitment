using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AspNetCoreSpa.Server.Entities;
using AspNetCoreSpa.Server.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AspNetCoreSpa.Server.Repositories
{
    public class EntityBaseRepository<T> : IEntityBaseRepository<T>
            where T : class, IEntityBase, new()
    {

        private readonly ApplicationDbContext _context;

        #region Properties
        public EntityBaseRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        #endregion
        public virtual IEnumerable<T> GetAll()
        {
            return _context.Set<T>().AsEnumerable();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public virtual IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public virtual async Task<IEnumerable<T>> AllIncludingAsync(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return await query.ToListAsync();
        }
        public T GetSingle(long id)
        {
            return _context.Set<T>().FirstOrDefault(x => x.Id == id);
        }

        public virtual T GetSingle(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().FirstOrDefault(predicate);
        }

        public T GetSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return query.Where(predicate).FirstOrDefault();
        }

        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> query = _context.Set<T>();

            return await _context.Set<T>().FirstOrDefaultAsync(predicate);
        }

        public async Task<T> GetSingleAsyncIncluding(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return await _context.Set<T>().Where(predicate).FirstOrDefaultAsync(predicate);
        }

        public virtual IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Where(predicate);
        }

        public virtual async Task<IList<T>> FindByIncludeAllAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).ToListAsync();
        }

        public virtual async Task<IList<T>> FindByAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).ToListAsync();
        }

        public virtual T Add(T entity)
        {
            EntityEntry dbEntityEntry = _context.Entry<T>(entity);
            var result = _context.Set<T>().Add(entity);
            _context.SaveChanges();
            return result.Entity;
        }

        public virtual void Edit(T entity)
        {
            EntityEntry dbEntityEntry = _context.Entry<T>(entity);
            dbEntityEntry.State = EntityState.Modified;
        }

        public virtual void Delete(T entity)
        {
            EntityEntry dbEntityEntry = _context.Entry<T>(entity);
            dbEntityEntry.State = EntityState.Deleted;
            _context.SaveChanges();
        }

        public virtual void CommitAsync()
        {
            _context.SaveChangesAsync();
        }

        public virtual void Commit()
        {
            _context.SaveChanges();
        }

        public virtual T GetSingleUsingNewContext(long id)
        {
            throw new NotImplementedException();
        }

        public virtual T UpdateWithNewContext(T entity)
        {
            throw new NotImplementedException();
        }
        public virtual void Update(T entity)
        {
            _context.Update(entity);
        }
    }
}
