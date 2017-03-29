using AspNetCoreSpa.Server.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quantium.Recruitment.Entities
{
    [Table(name: "Feedback")]
    public class Feedback : IEntityBase
    {
        public virtual long Id { get; set; }

        public virtual string Description { get; set; }

        public virtual long FeedbackTypeId { get; set; }

        public virtual long TestId { get; set; }

        public virtual long CandidateId { get; set; }

        [ForeignKey("FeedbackTypeId")]
        public virtual FeedbackType FeedbackType { get; set; }

        [ForeignKey("TestId")]
        public virtual Test Test { get; set; }

        [ForeignKey("CandidateId")]
        public virtual Candidate Candidate { get; set; }
    }
}
