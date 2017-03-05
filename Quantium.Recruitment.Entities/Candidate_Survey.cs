using AspNetCoreSpa.Server.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quantium.Recruitment.Entities
{
    [Table(name: "Candidate_Survey")]
    public class Candidate_Survey: IEntityBase
    {
        public virtual long Id { get; set; }
        public virtual long CandidateId { get; set; }
        public virtual long JobId { get; set; }

        [ForeignKey("CandidateId")]
        public virtual Candidate candidate { get; set; }

        [ForeignKey("JobId")]
        public virtual Job Job { get; set; }
        public virtual bool IsFinished { get; set; }

    }
}
