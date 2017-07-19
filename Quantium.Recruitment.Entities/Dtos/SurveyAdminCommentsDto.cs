using System;
using System.Collections.Generic;

namespace Quantium.Recruitment.Models
{
    public class SurveyAdminCommentsDto
    {
        public long Id { get; set; }

        public long ResponseId { get; set; }

        public long AdminId { get; set; }

        public DateTime DateTime { get; set; }     

        public string Comments { get; set; }

        public AdminDto Admin { get; set; }
       
        public SurveyResponseDto SurveyResponse { get; set; }

    }
}
