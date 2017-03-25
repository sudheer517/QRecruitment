using System.Linq;
using Quantium.Recruitment.Entities;
using AspNetCoreSpa.Server;
using AspNetCoreSpa.Server.Repositories;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System;

namespace Quantium.Recruitment.Infrastructure.Repositories
{
    //public interface IJobRepository : IGenericRepository<Job>
    //{
    //    Job FindById(long Id);
    //    void Update(Job entity);
    //}

    public class JobRepository : EntityBaseRepository<Job>
    {
        private readonly ApplicationDbContext _context;

        public JobRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public override IEnumerable<Job> GetAll()
        {
            IQueryable<Job> query = _context.Set<Job>();
            query =
                query.
                Include(job => job.Department).
                Include(job => job.JobDifficultyLabels).
                    ThenInclude(jdl => jdl.Difficulty).
                Include(job => job.JobDifficultyLabels).
                    ThenInclude(jdl => jdl.Label);

            return query.AsEnumerable();
        }

        public override async Task<IList<Job>> FindByAsync(Expression<Func<Job, bool>> predicate)
        {
            IQueryable<Job> query = _context.Set<Job>();
            query =
                query.
                Include(job => job.Department).
                Include(job => job.JobDifficultyLabels).
                    ThenInclude(jdl => jdl.Difficulty).
                Include(job => job.JobDifficultyLabels).
                    ThenInclude(jdl => jdl.Label);

            return await query.Where(predicate).ToListAsync();
        }

        public override void Delete(Job job)
        {
            var query = _context.Set<Job>();
            var entity = query.Include(j => j.JobDifficultyLabels).AsEnumerable().FirstOrDefault(j => j.Id == job.Id);
                
            _context.Remove(entity);
            _context.SaveChanges();
        }
    }
}