using System.Linq;

namespace Quantium.Recruitment.Infrastructure.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        void Add(T entity);

        void Delete(T entity);

        IQueryable<T> GetAll();
    }
}
