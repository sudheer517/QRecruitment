using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quantium.Recruitment.Entities
{
    [Table(name: "Challenge")]
    public class Challenge : Identifiable
    {
        public virtual long TestId { get; set; }

        public virtual long QuestionId { get; set; }

        public virtual DateTime? StartTime { get; set; }

        public virtual DateTime? AnsweredTime { get; set; }

        public virtual bool? IsSent { get; set; }

        public virtual bool? IsAnswered { get; set; }

        [ForeignKey("TestId")]
        public virtual Test Test { get; set; }

        [ForeignKey("QuestionId")]
        public virtual Question Question { get; set; }

        public virtual List<CandidateSelectedOption> CandidateSelectedOptions { get; set; }

    }
}
