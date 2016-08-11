using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quantium.Recruitment.Entities
{
    public class Challenge : Identifiable
    {
        public virtual long TestId { get; set; }

        public virtual long QuestionId { get; set; }

        public virtual DateTime StartTime { get; set; }

        public virtual DateTime AnsweredTime { get; set; }

        [ForeignKey("TestId")]
        public Test Test { get; set; }

        [ForeignKey("QuestionId")]
        public Question Question { get; set; }

        public List<CandidateSelectedOption> CandidateSelectedOptions { get; set; }

    }
}
