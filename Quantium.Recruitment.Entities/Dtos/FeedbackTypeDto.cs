using AspNetCoreSpa.Server.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quantium.Recruitment.Entities
{
    public class FeedbackTypeDto
    {
        public long Id { get; set; }

        public string Name { get; set; }
    }
}
