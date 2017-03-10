namespace Quantium.Recruitment.Models
{
    public class SurveyChallengeDto
    {
        public long Id { get; set; }

        public long ChallengeId { get; set; }

        public long SurveyQuestionId { get; set; }

        public SurveyQuestionDto SurveyQuestion { get; set; }

        public string CandidateAnswer { get; set; }

        public string TotalTestTimeInMinutes { get; set; }
    }
}