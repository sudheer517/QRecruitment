using AspNetCoreSpa.Server.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quantium.Recruitment.Entities
{
    [Table(name: "Candidate")]
    public class Candidate: IEntityBase
    {
        public virtual long Id { get; set; }

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

        public virtual double CGPA { get; set; }

        public virtual double ExperienceInYears { get; set; }

        public virtual string CurrentCompany { get; set; }

        public virtual bool IsInformationFilled { get; set; }

        public virtual bool PasswordSent { get; set; }
       
        public virtual int TestMailSent { get; set; }

        public DateTime? CreatedUtc { get; set; }

        public virtual long AdminId { get; set; }

        [ForeignKey("AdminId")]
        public virtual Admin Admin { get; set; }

        public virtual ICollection<Candidate_Job> CandidateJobs { get; set; }

        public virtual ICollection<Survey> Surveys { get; set; }

        public virtual ICollection<Test> Tests { get; set; }
    }
}
