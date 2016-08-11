using System.Collections.Generic;

namespace Quantium.Recruitment.Entities
{
    public class QuestionGroup : Identifiable
    {
        public virtual string Description { get; set; }

        public List<Question> Questions { get; set; }
    }
}
