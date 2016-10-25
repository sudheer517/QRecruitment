using System.Collections.Generic;
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

        public virtual string Branch { get; set; }

        public virtual string College { get; set; }

        public virtual string PassingYear { get; set; }

        public virtual double ExperienceInYears { get; set; }

        public virtual string CurrentCompany { get; set; }

        public virtual bool IsInformationFilled { get; set; }

        public virtual List<Candidate_Job> CandidateJobs { get; set; }

        public virtual List<Test> Tests { get; set; }
    }
}
