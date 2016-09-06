using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quantium.Recruitment.Entities;

namespace Quantium.Recruitment.ApiServices.Models
{
    public class QuestionDto : Identifiable
    {
        public long QuestionGroupId { get; set; }
        public string Text { get; set; }

        public byte[] Image { get; set; }

        public int TimeInSeconds { get; set; }

        public QuestionGroupDto QuestionGroup { get; set; }

        public List<OptionDto> Options { get; set; }

        //public List<ChallengeDto> Challenges { get; set; }
    }
}