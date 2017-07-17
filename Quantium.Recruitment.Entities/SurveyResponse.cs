using AspNetCoreSpa.Server.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace Quantium.Recruitment.Entities
{

    [Table(name: "SurveyResponse")]
    public class SurveyResponse : IEntityBase
    {
        public virtual long Id { get; set; }
        public virtual long CandidateId { get; set; }

        public virtual long SurveyQuestionId { get; set; }

        public virtual string Response { get; set; }

        [ForeignKey("CandidateId")]
        public virtual Candidate Candidate { get; set; }
        [ForeignKey("SurveyQuestionId")]
        public virtual SurveyQuestion SurveyQuestion { get; set; }
    }
}
