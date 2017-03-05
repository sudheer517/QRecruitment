using AspNetCoreSpa.Server.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quantium.Recruitment.Entities
{
    [Table(name: "Difficulty")]
    public class Difficulty : IEntityBase
    {
        public virtual long Id { get; set; }
        public virtual string Name { get; set; }

        //public virtual List<Job_Label_Difficulty> JobDifficultyLabels { get; set; }
    }
}
