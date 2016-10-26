using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quantium.Recruitment.Entities
{
    [Table(name: "Test")]
    public class Test : Identifiable
    {
        public virtual string Name { get; set; }

        public virtual long JobId { get; set; }

        public virtual long CandidateId { get; set; }

        public virtual List<Challenge> Challenges { get; set; }

        public virtual List<Test_Label> TestLabels { get; set; }

        [ForeignKey("CandidateId")]
        public virtual Candidate Candidate { get; set; }

        [ForeignKey("JobId")]
        public virtual Job Job { get; set; }

        public virtual bool IsFinished { get; set; }
    }
}
