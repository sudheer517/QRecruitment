using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quantium.Recruitment.Entities
{

    [Table(name: "SurveyQuestion")]
    public class SurveyQuestion : Identifiable
    {
        public virtual string Text { get; set; }
        
        public virtual List<SurveyChallenge> SurveyChallenges { get; set; }

        public virtual long CreatedByUserId { get; set; }
    }
}
