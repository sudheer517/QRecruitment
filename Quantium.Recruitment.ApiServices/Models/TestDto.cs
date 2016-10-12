using System.Collections.Generic;

namespace Quantium.Recruitment.ApiServices.Models
{
    public class TestDto
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public List<ChallengeDto> Challenges { get; set; }

        public List<Test_LabelDto> TestLabels { get; set; }
    }
}
