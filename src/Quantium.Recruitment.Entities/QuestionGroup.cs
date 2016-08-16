using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quantium.Recruitment.Entities
{
    [Table(name: "QuestionGroup")]
    public class QuestionGroup : Identifiable
    {
        public virtual string Description { get; set; }

        public virtual List<Question> Questions { get; set; }
    }
}
