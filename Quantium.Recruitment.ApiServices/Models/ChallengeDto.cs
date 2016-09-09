using System;
using System.Collections.Generic;

namespace Quantium.Recruitment.ApiServices.Models
{
    public class ChallengeDto
    {
        public long Id { get; set; }

        public long TestId { get; set; }

        public long QuestionId { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime AnsweredTime { get; set; }

        //public TestDto Test { get; set; }

        public QuestionDto Question { get; set; }

        public List<CandidateSelectedOptionDto> CandidateSelectedOptions { get; set; }

    }
}
