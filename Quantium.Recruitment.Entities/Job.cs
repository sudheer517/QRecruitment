using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quantium.Recruitment.Entities
{
    [Table(name: "Job")]
    public class Job : Identifiable
    {
        public virtual string Title { get; set; }

        public virtual string Profile { get; set; }

        public virtual long DepartmentId { get; set; }

        public virtual long CreatedByUserId { get; set; }

        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }

        public virtual List<Job_Difficulty_Label> JobDifficultyLabels { get; set; }

        public virtual List<Candidate_Job> CandidateJobs { get; set; }

        public virtual List<Test> Tests { get; set; }

        public virtual DateTime? CreatedUtc { get; set; }
    }
}
