using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quantium.Recruitment.Entities
{
    public class Question : Identifiable
    {
        public virtual long QuestionGroupId { get; set; }
        public virtual string Text { get; set; }

        public virtual byte[] Image { get; set; }

        public virtual int TimeInSeconds { get; set; }

        [ForeignKey("QuestionGroupId")]
        public QuestionGroup QuestionGroup { get; set; }

        public List<Option> Options { get; set; }

        public List<Challenge> Challenges { get; set; }
    }
}
