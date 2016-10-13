using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quantium.Recruitment.Entities;

namespace Quantium.Recruitment.ApiServices.Models
{
    public class Job_Difficulty_LabelDto
    {
        public long Id { get; set; }

        public DifficultyDto Difficulty { get; set; }

        public LabelDto Label { get; set; }

        public int QuestionCount { get; set; }
    }
}