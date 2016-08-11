using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quantium.Recruitment.Entities
{
    public class CandidateSelectedOption : Identifiable
    {
        public virtual long ChallengeId { get; set; }

        public virtual long OptionId { get; set; }

        [ForeignKey("ChallengeId")]
        public Challenge Challenge { get; set; }

        [ForeignKey("OptionId")]
        public List<Option> Options { get; set; }
    }
}
