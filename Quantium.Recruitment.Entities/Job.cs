using AspNetCoreSpa.Server.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quantium.Recruitment.Entities
{
    [Table(name: "Job")]
    public class Job : IEntityBase
    {
        public virtual long Id { get; set; }

        public virtual string Title { get; set; }

        public virtual string Profile { get; set; }

        public virtual long DepartmentId { get; set; }

        public virtual long CreatedByUserId { get; set; }

        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }

        public virtual ICollection<Job_Difficulty_Label> JobDifficultyLabels { get; set; }

        public virtual ICollection<Candidate_Job> CandidateJobs { get; set; }

        public virtual ICollection<Test> Tests { get; set; }

        public virtual ICollection<Survey> Surveys { get; set; }

        public virtual DateTime? CreatedUtc { get; set; }

        public virtual bool? IsActive { get; set; }
    }
}
