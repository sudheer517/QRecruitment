using AspNetCoreSpa.Server.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quantium.Recruitment.Entities
{
    [Table(name: "Label")]
    public class Label : IEntityBase
    {
        public virtual long Id { get; set; }

        public virtual string Name { get; set; }

        public virtual ICollection<Test_Label> TestLabels { get; set; }

        public virtual ICollection<Job_Difficulty_Label> JobDifficultyLabels { get; set; }
    }
}
