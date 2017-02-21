using System.Collections.Generic;


namespace Quantium.Recruitment.Models
{
    public class SurveyDto
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public List<SurveyChallengeDto> SurveyChallenges { get; set; }

        public CandidateDto Candidate { get; set; }

    }
}
