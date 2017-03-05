using AspNetCoreSpa.Server.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quantium.Recruitment.Entities
{
    [Table(name: "Candidate_Job")]
    public class Candidate_Job : IEntityBase
    {
        public virtual long Id { get; set; }
        public virtual long CandidateId { get; set; }

        public virtual long JobId { get; set; }

        [ForeignKey("CandidateId")]
        public virtual Candidate Candidate { get; set; }

        [ForeignKey("JobId")]
        public virtual Job Job { get; set; }

        public virtual bool IsFinished { get; set; }

    }
}
