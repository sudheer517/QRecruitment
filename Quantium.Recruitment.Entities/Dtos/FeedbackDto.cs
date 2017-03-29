using AspNetCoreSpa.Server.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quantium.Recruitment.Entities
{
    public class FeedbackDto
    {
        public long Id { get; set; }

        public string Description { get; set; }

        public long FeedbackTypeId { get; set; }

        public long TestId { get; set; }

        public long CandidateId { get; set; }
    }
}
