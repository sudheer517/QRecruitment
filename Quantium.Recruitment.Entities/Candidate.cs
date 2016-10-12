using System.ComponentModel.DataAnnotations.Schema;

namespace Quantium.Recruitment.Entities
{
    [Table(name: "Candidate")]
    public class Candidate: Identifiable
    {
        public virtual string FirstName { get; set; }

        public virtual string LastName { get; set; }

        public virtual string Email { get; set; }

        public virtual long Mobile { get; set; }

        public virtual bool IsActive { get; set; }

        public virtual string City { get; set; }

        public virtual string State { get; set; }

        public virtual string Country { get; set; }

        public virtual string College { get; set; }
    }
}
