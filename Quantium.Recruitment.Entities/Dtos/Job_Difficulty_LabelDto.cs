using System;
using System.Collections.Generic;
using System.Linq;
using Quantium.Recruitment.Entities;

namespace Quantium.Recruitment.Models
{
    public class Job_Difficulty_LabelDto
    {
        public long Id { get; set; }

        public DifficultyDto Difficulty { get; set; }

        public LabelDto Label { get; set; }

        public int DisplayQuestionCount { get; set; }

        public int PassingQuestionCount { get; set; }

        public int AnsweredCount { get; set; }
    }
}