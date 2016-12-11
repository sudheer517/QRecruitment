using System;
using System.Collections.Generic;

namespace Quantium.Recruitment.Models
{
    public class ChallengeDto
    {
        public long Id { get; set; }

        public long TestId { get; set; }

        public long QuestionId { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime AnsweredTime { get; set; }

        public int RemainingChallenges { get; set; }

        public int currentChallenge { get; set; }

        public QuestionDto Question { get; set; }

        public bool[] ChallengesAnswered { get; set; }

        public List<CandidateSelectedOptionDto> CandidateSelectedOptions { get; set; }

        public string TotalTestTimeInMinutes { get; set; }

        public string RemainingTestTimeInMinutes { get; set; }

    }
}
