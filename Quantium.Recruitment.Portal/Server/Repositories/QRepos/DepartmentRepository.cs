using System.Linq;
using Quantium.Recruitment.Entities;
using AspNetCoreSpa.Server.Repositories;
using AspNetCoreSpa.Server;

namespace Quantium.Recruitment.Infrastructure.Repositories
{
    //public interface IDepartmentRepository : IGenericRepository<Department>
    //{
    //    Department FindById(long Id);
    //    void Update(Department entity);
    //}

    public class DepartmentRepository : EntityBaseRepository<Department>
    {
        public DepartmentRepository(ApplicationDbContext context) : base(context)
        {
        }

        //private readonly IRecruitmentContext _dbContext;
        //public DepartmentRepository(IRecruitmentContext dbContext) : base(dbContext)
        //{
        //    _dbContext = dbContext;
        //}

        //public Department FindById(long Id)
        //{
        //    return _dbContext.Departments.Single(entity => entity.Id == Id);
        //}

        //public void Update(Department entity)
        //{
        //    _dbContext.Departments.Add(entity);
        //}
    }
}