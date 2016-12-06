using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quantium.Recruitment.Entities;

namespace Quantium.Recruitment.ApiServices.Models
{
    public class Question_Difficulty_LabelDto
    {
        public long DifficultyId { get; set; }

        public long LabelId { get; set; }

        public int QuestionCount { get; set; }
    }
}