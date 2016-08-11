using System.ComponentModel.DataAnnotations.Schema;

namespace Quantium.Recruitment.Entities
{
    public class Candidate: Identifiable
    {
        public virtual string FirstName { get; set; }

        public virtual string LastName { get; set; }

        public virtual string Email { get; set; }

        public virtual long Mobile { get; set; }

        public virtual bool IsActive { get; set; }

        public virtual long JobId { get; set; }

        public virtual long TestId { get; set; }

        [ForeignKey("JobId")]
        public Job Job { get; set; }

        [ForeignKey("TestId")]
        public Test Test { get; set; }
    }
}
