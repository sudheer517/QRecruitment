using System.Collections.Generic;
using System.Linq;
using Quantium.Recruitment.Entities;

namespace Quantium.Recruitment.Infrastructure.Repositories
{
    public interface IAdminRepository : IRepository<Admin>
    {
    }

    public class AdminRepository : IAdminRepository
    {
        private readonly IRecruitmentContext _dbContext;
        public AdminRepository(IRecruitmentContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(Admin entity)
        {
            _dbContext.Admins.Add(entity);
        }

        public void Delete(Admin entity)
        {
            _dbContext.Admins.Remove(entity);
        }

        public Admin FindById(long Id)
        {
            return _dbContext.Admins.Single(entity => entity.Id == Id);
        }

        public IEnumerable<Admin> GetAll()
        {
            return _dbContext.Admins;
        }

        public void Update(Admin entity)
        {
            _dbContext.Admins.Add(entity);
        }
    }
}