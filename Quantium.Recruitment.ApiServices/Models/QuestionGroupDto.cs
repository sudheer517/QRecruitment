using System.Collections.Generic;
using Reinforced.Typings.Attributes;

namespace Quantium.Recruitment.ApiServices.Models
{
    [TsClass]
    public class QuestionGroupDto
    {
        public long Id { get; set; }

        public string Description { get; set; }

        //public List<QuestionDto> Questions { get; set; }
    }
}
