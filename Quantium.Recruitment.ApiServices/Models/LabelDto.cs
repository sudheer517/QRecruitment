
using System.Collections.Generic;

namespace Quantium.Recruitment.ApiServices.Models
{
    public class LabelDto
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public virtual List<Question_Label_DifficultyDto> DifficultyLabels { get; set; }
    }
}
