using AspNetCoreSpa.Server.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace Quantium.Recruitment.Entities
{

    [Table(name: "SurveyAdminComments")]
    public class SurveyAdminComments : IEntityBase
    {
        public virtual long Id { get; set; }
        public virtual long AdminId { get; set; }

        public virtual DateTime DateTime { get; set; }

        public virtual string Comments { get; set; }

        [ForeignKey("AdminId")]
        public virtual Admin Admin { get; set; }
        [ForeignKey("Id")]
        public virtual SurveyResponse SurveyResponse { get; set; }
    }
}
