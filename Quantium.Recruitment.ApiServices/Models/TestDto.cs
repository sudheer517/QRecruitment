﻿using System.Collections.Generic;

namespace Quantium.Recruitment.ApiServices.Models
{
    public class TestDto : Identifiable
    {
        public string Name { get; set; }

        public List<LabelDto> Labels { get; set; }

        public List<ChallengeDto> Challenges { get; set; }

        public List<CandidateDto> Candidates { get; set; }
    }
}
