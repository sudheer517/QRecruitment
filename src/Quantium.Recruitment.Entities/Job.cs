using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quantium.Recruitment.Entities
{
    [Table(name: "Job")]
    public class Job : Identifiable
    {
        public virtual string Title { get; set; }

        public virtual string Profile { get; set; }

        public virtual long DepartmentId { get; set; }

        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }

        public virtual List<Candidate> Candidates { get; set; }

        public virtual List<Label> Labels { get; set; }
    }
}
