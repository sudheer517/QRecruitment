using System;
using System.Collections.Generic;
using System.Text;

namespace Quantium.Recruitment.Models
{
    public class TestResultDto
    {
        public long Id { get; set; }

        public string Candidate { get; set; }

        public string Email { get; set; }

        public string JobApplied { get; set; }

        public DateTime? FinishedDate { get; set; }

        public string Result { get; set; }

        public string College { get; set; }

        public double CGPA { get; set; }

        public int TotalRightAnswers { get; set; }

        public bool IsFinished { get; set; }
    }
}
