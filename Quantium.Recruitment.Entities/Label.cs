using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quantium.Recruitment.Entities
{
    [Table(name: "Label")]
    public class Label : Identifiable
    {
        public virtual string Name { get; set; }

        public virtual List<Test_Label> TestLabels { get; set; }

        public virtual List<Job_Difficulty_Label> JobDifficultyLabels { get; set; }
    }
}
