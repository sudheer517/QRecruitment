using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quantium.Recruitment.Entities
{
    public class Job : Identifiable
    {
        public virtual string Title { get; set; }

        public virtual string Profile { get; set; }

        public virtual long DepartmentId { get; set; }

        [ForeignKey("DepartmentId")]
        public Department Department { get; set; }

        public List<Candidate> Candidates { get; set; }

        public List<Label> Labels { get; set; }
    }
}
