using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quantium.Recruitment.Entities
{
    [Table(name: "Question_Label_Difficulty")]
    public class Question_Label_Difficulty : Identifiable
    {
        public virtual Question Question { get; set; }

        public virtual Label Label { get; set; }

        public virtual Difficulty Difficulty { get; set; }
    }
}
