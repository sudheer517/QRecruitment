﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quantium.Recruitment.Entities;

namespace Quantium.Recruitment.Models
{
    public class QuestionDto
    {
        public long Id { get; set; }

        public string Text { get; set; }

        public string ImageUrl { get; set; }

        public int TimeInSeconds { get; set; }

        public bool RandomizeOptions { get; set; }

        public QuestionGroupDto QuestionGroup { get; set; }

        public List<OptionDto> Options { get; set; }

        public DifficultyDto Difficulty { get; set; }

        public LabelDto Label { get; set; }
    }
}