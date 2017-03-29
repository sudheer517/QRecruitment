using System.Linq;
using Quantium.Recruitment.Entities;
using AspNetCoreSpa.Server;
using AspNetCoreSpa.Server.Repositories;

namespace Quantium.Recruitment.Infrastructure.Repositories
{
    public class FeedbackRepository : EntityBaseRepository<Feedback>
    {
        public FeedbackRepository(ApplicationDbContext context) : base(context)
        {
        }

    }
}