﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quantium.Recruitment.Entities
{
    [Table(name: "Survey")]
    public class Survey : Identifiable
    {
        public virtual string Name { get; set; }

        public virtual long JobId { get; set; }

        public virtual long CandidateId { get; set; }

        public virtual long CreatedByUserId { get; set; }

        public virtual List<SurveyChallenge> SurveyChallenges { get; set; }

        public virtual List<Test_Label> TestLabels { get; set; }

        [ForeignKey("CandidateId")]
        public virtual Candidate Candidate { get; set; }

        [ForeignKey("JobId")]
        public virtual Job Job { get; set; }

        public virtual bool IsFinished { get; set; }

        public virtual DateTime? FinishedDate { get; set; }

        public virtual DateTime? CreatedUtc { get; set; }

        public virtual bool? IsArchived { get; set; }
    }
}
