using AspNetCoreSpa.Server.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quantium.Recruitment.Entities
{
    [Table(name: "QuestionGroup")]
    public class QuestionGroup : IEntityBase
    {
        public virtual long Id { get; set; }

        public virtual string Description { get; set; }

        public virtual string ImageUrl { get; set; }

        public virtual ICollection<Question> Questions { get; set; }
    }
}
