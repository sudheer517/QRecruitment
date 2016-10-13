using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quantium.Recruitment.Entities
{
    [Table(name: "Question")]
    public class Question : Identifiable
    {
        public virtual long? QuestionGroupId { get; set; }

        public virtual string Text { get; set; }

        public virtual string ImageUrl { get; set; }

        public virtual int TimeInSeconds { get; set; }

        public virtual bool? RandomizeOptions { get; set; }

        public virtual long? DifficultyId { get; set; }

        public virtual long? LabelId { get; set; }

        [ForeignKey("QuestionGroupId")]
        public virtual QuestionGroup QuestionGroup { get; set; }

        [ForeignKey("DifficultyId")]
        public virtual Difficulty Difficulty { get; set; }

        [ForeignKey("LabelId")]
        public virtual Label Label { get; set; }

        public virtual List<Option> Options { get; set; }

        public virtual List<Challenge> Challenges { get; set; }
    }
}
