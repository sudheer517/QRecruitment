using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quantium.Recruitment.Entities;

namespace Quantium.Recruitment.Infrastructure.Repositories
{
    public interface IRepository<T> where T : Identifiable
    {
        void Add(T entity);

        void Delete(T entity);

        void Update(T entity);

        T FindById(long Id);

        IEnumerable<T> GetAll();
    }
}
