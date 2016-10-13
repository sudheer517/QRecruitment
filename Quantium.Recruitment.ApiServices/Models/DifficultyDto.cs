﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quantium.Recruitment.Entities;

namespace Quantium.Recruitment.ApiServices.Models
{
    public class DifficultyDto
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public List<Job_Label_DifficultyDto> JobDifficultyLabels { get; set; }
    }
}