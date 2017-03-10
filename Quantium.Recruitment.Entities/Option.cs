using AspNetCoreSpa.Server.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quantium.Recruitment.Entities
{
    [Table(name: "Option")]
    public class Option : IEntityBase
    {
        public virtual long Id { get; set; }
        public virtual long QuestionId { get; set; }

        public virtual string Text { get; set; }

        public virtual string ImageUrl { get; set; }

        public virtual bool IsAnswer { get; set; }

        [ForeignKey("QuestionId")]
        public virtual Question Question { get; set; }

        public virtual CandidateSelectedOption CandidateSelectedOption { get; set; }
    }
}
