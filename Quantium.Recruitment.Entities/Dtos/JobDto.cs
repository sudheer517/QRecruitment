using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Quantium.Recruitment.Entities;

namespace Quantium.Recruitment.Models
{
    public class JobDto
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public string Profile { get; set; }

        public DepartmentDto Department { get; set; }

        public List<Job_Difficulty_LabelDto> JobDifficultyLabels { get; set; }

        public long CreatedByUserId { get; set; }
    }
}
