using System.Linq;
using Quantium.Recruitment.Entities;
using AspNetCoreSpa.Server;
using AspNetCoreSpa.Server.Repositories;

namespace Quantium.Recruitment.Infrastructure.Repositories
{
    public class FeedbackTypeRepository : EntityBaseRepository<FeedbackType>
    {
        public FeedbackTypeRepository(ApplicationDbContext context) : base(context)
        {
        }

    }
}