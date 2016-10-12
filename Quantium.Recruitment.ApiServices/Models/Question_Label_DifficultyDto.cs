using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quantium.Recruitment.Entities;

namespace Quantium.Recruitment.ApiServices.Models
{
    public class Question_Label_DifficultyDto
    {
        public long Id { get; set; }

        public QuestionDto Question { get; set; }

        public LabelDto Label { get; set; }

        public DifficultyDto Difficulty { get; set; }
    }
}