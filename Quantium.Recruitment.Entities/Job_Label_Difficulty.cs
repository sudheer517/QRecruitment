using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quantium.Recruitment.Entities
{
    [Table(name: "Job_Difficulty_Label")]
    public class Job_Difficulty_Label : Identifiable
    {
        public virtual Job Job { get; set; }

        public virtual Difficulty Difficulty { get; set; }

        public virtual Label Label { get; set; }

        public virtual int QuestionCount { get; set; }
    }
}
