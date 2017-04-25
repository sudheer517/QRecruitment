using System;
using System.Collections.Generic;

namespace Quantium.Recruitment.Models
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

        public DateTime FinishedDate { get; set; }

        public int TotalRightAnswers { get; set; }

        public int TotalChallengesDisplayed { get; set; }

        public int TotalChallengesAnswered { get; set; }

        public bool IsTestPassed { get; set; }

        public long CreatedByUserId { get; set; }

        public DateTime CreatedUtc { get; set; }

        public Dictionary<string, string> LabelDiffAnswers { get; set; }
    }
}
