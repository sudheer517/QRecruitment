using System.Collections.Generic;
using System.Linq;
using Quantium.Recruitment.Entities;

namespace Quantium.Recruitment.Infrastructure.Repositories
{
    public interface IDepartmentRepository : IRepository<Department>
    {
    }

    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly IRecruitmentContext _dbContext;
        public DepartmentRepository(IRecruitmentContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(Department entity)
        {
            _dbContext.Departments.Add(entity);
        }

        public void Delete(Department entity)
        {
            _dbContext.Departments.Remove(entity);
        }

        public Department FindById(long Id)
        {
            return _dbContext.Departments.Single(entity => entity.Id == Id);
        }

        public IEnumerable<Department> GetAll()
        {
            return _dbContext.Departments;
        }

        public void Update(Department entity)
        {
            _dbContext.Departments.Add(entity);
        }
    }
}