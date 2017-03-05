using System.Linq;
using Quantium.Recruitment.Entities;
using System;
using AspNetCoreSpa.Server.Repositories;
using AspNetCoreSpa.Server;
using AspNetCoreSpa.Server.Repositories.Abstract;
using System.Runtime.Loader;

namespace Quantium.Recruitment.Infrastructure.Repositories
{
    //public interface IAdminRepository : IEntityBaseRepository<Admin>
    //{
    //}

    public class AdminRepository : EntityBaseRepository<Admin>
    {
        public AdminRepository(ApplicationDbContext context) : base(context)
        {
        }

        //public Admin FindById(long Id)
        //{
        //    return _dbContext.Admins.Single(entity => entity.Id == Id);
        //}

        //public void Update(Admin entity)
        //{
        //    _dbContext.Admins.AddOrUpdate(entity);
        //}
    }
}