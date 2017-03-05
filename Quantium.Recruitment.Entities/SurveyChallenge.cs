using AspNetCoreSpa.Server.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quantium.Recruitment.Entities
{
    [Table (name: "SurveyChallenge")]
    public class SurveyChallenge : IEntityBase
    {
        public virtual long Id { get; set; }

        public virtual long SurveyId { get; set; }

        public virtual long SurveyQuestionId { get; set; }

        public virtual bool? IsSent { get; set; }

        public virtual bool? IsAnswered { get; set; }

        [ForeignKey("SurveyId")]
        public virtual Survey Survey { get; set; }

        public virtual string CandidateAnswer { get; set; }

        [ForeignKey("SurveyQuestionId")]
        public virtual SurveyQuestion SurveyQuestion { get; set; }
    }
}
