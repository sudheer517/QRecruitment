using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quantium.Recruitment.Entities
{
    [Table(name: "Difficulty")]
    public class Difficulty : Identifiable
    {
        public virtual string Name { get; set; }

        public virtual List<Question_Label_Difficulty> DifficultyLabels { get; set; }
    }
}
