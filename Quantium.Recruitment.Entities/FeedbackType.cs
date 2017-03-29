using AspNetCoreSpa.Server.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quantium.Recruitment.Entities
{
    [Table(name: "FeedbackType")]
    public class FeedbackType : IEntityBase
    {
        public virtual long Id { get; set; }

        public virtual string Name { get; set; }

    }
}
