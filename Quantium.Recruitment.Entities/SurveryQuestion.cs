using AspNetCoreSpa.Server.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quantium.Recruitment.Entities
{

    [Table(name: "SurveyQuestion")]
    public class SurveyQuestion : IEntityBase
    {
        public virtual long Id { get; set; }

        public virtual string Text { get; set; }
        
        public virtual ICollection<SurveyChallenge> SurveyChallenges { get; set; }

        public virtual long CreatedByUserId { get; set; }
    }
}
