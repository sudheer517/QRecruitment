using System.Collections.Generic;


namespace Quantium.Recruitment.Models
{
    public class Survey
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public List<SurveyChallengeDto> SurveryChallenges { get; set; }

        public CandidateDto Candidate { get; set; }

    }
}
