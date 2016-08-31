using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quantium.Recruitment.Entities
{
    [Table(name: "Option")]
    public class Option : Identifiable
    {
        public virtual long QuestionId { get; set; }

        public virtual string Text { get; set; }

        public virtual byte[] Image { get; set; }

        public virtual bool IsAnswer { get; set; }

        [ForeignKey("QuestionId")]
        public virtual Question Question { get; set; }
    }
}
