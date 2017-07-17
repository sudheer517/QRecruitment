using System.Collections.Generic;

namespace Quantium.Recruitment.Models
{
    public class SurveyResponseDto
    {
        public long Id { get; set; }

        public long CandidateId { get; set; }

        public long QuestionId { get; set; }     

        public string Response { get; set; }

        public CandidateDto Candidate { get; set; }

        public SurveyQuestionDto SurveyQuestion { get; set; }

    }
}
