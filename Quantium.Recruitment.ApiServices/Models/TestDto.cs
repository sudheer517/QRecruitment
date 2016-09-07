using System.Collections.Generic;
using Reinforced.Typings.Attributes;

namespace Quantium.Recruitment.ApiServices.Models
{
    [TsClass]
    public class TestDto
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public List<LabelDto> Labels { get; set; }

        public List<ChallengeDto> Challenges { get; set; }

        public List<CandidateDto> Candidates { get; set; }
    }
}
