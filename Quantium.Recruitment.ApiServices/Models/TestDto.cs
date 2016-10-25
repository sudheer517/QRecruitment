using System.Collections.Generic;

namespace Quantium.Recruitment.ApiServices.Models
{
    public class TestDto
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public List<ChallengeDto> Challenges { get; set; }

        public List<Test_LabelDto> TestLabels { get; set; }

        public CandidateDto Candidate { get; set; }

        public JobDto Job { get; set; }

        public bool IsFinished { get; set; }
    }
}
