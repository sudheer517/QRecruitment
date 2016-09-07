using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quantium.Recruitment.Entities;
using Reinforced.Typings.Attributes;

namespace Quantium.Recruitment.ApiServices.Models
{
    [TsClass]
    public class QuestionDto
    {
        public long Id { get; set; }

        public long QuestionGroupId { get; set; }

        public string Text { get; set; }

        public byte[] Image { get; set; }

        public int TimeInSeconds { get; set; }

        public QuestionGroupDto QuestionGroup { get; set; }

        public List<OptionDto> Options { get; set; }

        //public List<ChallengeDto> Challenges { get; set; }
    }
}