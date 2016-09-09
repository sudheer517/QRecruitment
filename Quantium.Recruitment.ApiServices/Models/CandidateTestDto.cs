using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quantium.Recruitment.ApiServices.Models
{
    public class CandidateTestDto
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public List<ChallengeDto> Questions { get; set; }
    }
}