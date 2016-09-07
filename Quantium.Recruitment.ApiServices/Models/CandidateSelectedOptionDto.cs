using Reinforced.Typings.Attributes;

namespace Quantium.Recruitment.ApiServices.Models
{
    [TsClass]
    public class CandidateSelectedOptionDto : Identifiable
    {
        public long ChallengeId { get; set; }

        public long OptionId { get; set; }

        //public ChallengeDto Challenge { get; set; }

        //public OptionDto Option { get; set; }
    }
}
