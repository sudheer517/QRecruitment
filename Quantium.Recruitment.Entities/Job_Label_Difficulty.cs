using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quantium.Recruitment.Entities
{
    [Table(name: "Job_Label_Difficulty")]
    public class Job_Label_Difficulty : Identifiable
    {
        public virtual Job Job { get; set; }

        public virtual Label Label { get; set; }

        public virtual Difficulty Difficulty { get; set; }
    }
}
