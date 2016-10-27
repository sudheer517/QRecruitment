using System.Data.Entity.Migrations;
using System.Linq;
using Quantium.Recruitment.Entities;
using System;

namespace Quantium.Recruitment.Infrastructure.Repositories
{
    public interface IAdminRepository : IGenericRepository<Admin>
    {
        Admin FindById(long Id);
        void Update(Admin entity);
    }

    public class AdminRepository : GenericRepository<Admin>, IAdminRepository
    {
        private readonly IRecruitmentContext _dbContext;
        public AdminRepository(IRecruitmentContext dbContext): base(dbContext)
        {
            _dbContext = dbContext;
        }

        public Admin FindById(long Id)
        {
            return _dbContext.Admins.Single(entity => entity.Id == Id);
        }

        public void Update(Admin entity)
        {
            _dbContext.Admins.AddOrUpdate(entity);
        }
    }
}