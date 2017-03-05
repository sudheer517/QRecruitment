using AspNetCoreSpa.Server.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quantium.Recruitment.Entities
{
    [Table(name: "Job_Difficulty_Label")]
    public class Job_Difficulty_Label : IEntityBase
    {
        public virtual long Id { get; set; }

        public virtual long JobId { get; set; }

        public virtual long DifficultyId { get; set; }

        public virtual long LabelId { get; set; }

        [ForeignKey("JobId")]
        public virtual Job Job { get; set; }

        [ForeignKey("DifficultyId")]
        public virtual Difficulty Difficulty { get; set; }

        [ForeignKey("LabelId")]
        public virtual Label Label { get; set; }

        public virtual int DisplayQuestionCount { get; set; }

        public virtual int PassingQuestionCount { get; set; }
    }
}
