using System;
using System.Collections.Generic;
using System.Linq;
using Quantium.Recruitment.Entities;

namespace Quantium.Recruitment.Models
{
    public class DifficultyDto
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public List<Job_Difficulty_LabelDto> JobDifficultyLabels { get; set; }
    }
}