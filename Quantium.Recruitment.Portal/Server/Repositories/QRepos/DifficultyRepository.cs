using System.Linq;
using Quantium.Recruitment.Entities;
using AspNetCoreSpa.Server;
using AspNetCoreSpa.Server.Repositories;

namespace Quantium.Recruitment.Infrastructure.Repositories
{
    //public interface IDifficultyRepository : IGenericRepository<Difficulty>
    //{
    //    Difficulty FindById(long Id);
    //    void Update(Difficulty entity);
    //    Difficulty FindByName(string name);
    //}

    public class DifficultyRepository : EntityBaseRepository<Difficulty>
    {
        public DifficultyRepository(ApplicationDbContext context) : base(context)
        {
        }

        //private readonly IRecruitmentContext _dbContext;
        //public DifficultyRepository(IRecruitmentContext dbContext) : base(dbContext)
        //{
        //    _dbContext = dbContext;
        //}

        //public Difficulty FindById(long Id)
        //{
        //    return _dbContext.Difficulties.Single(entity => entity.Id == Id);
        //}

        //public Difficulty FindByName(string name)
        //{
        //    return _dbContext.Difficulties.SingleOrDefault(entity => entity.Name == name);

        //}
        //public void Update(Difficulty entity)
        //{
        //    _dbContext.Difficulties.Add(entity);
        //}
    }
}