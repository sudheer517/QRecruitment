using System.Collections.Generic;

namespace Quantium.Recruitment.ApiServices.Models
{
    public class TestDto
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public List<LabelDto> Labels { get; set; }

        public List<QuestionDto> Questions { get; set; }

        //public List<CandidateDto> Candidates { get; set; }
    }
}
