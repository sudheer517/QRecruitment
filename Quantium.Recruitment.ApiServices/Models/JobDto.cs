using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Quantium.Recruitment.Entities;

namespace Quantium.Recruitment.ApiServices.Models
{
    public class JobDto : Entities.Identifiable
    {
        public string Title { get; set; }

        public string Profile { get; set; }

        public long DepartmentId { get; set; }

        public DepartmentDto Department { get; set; }

        public List<CandidateDto> Candidates { get; set; }

        public List<LabelDto> Labels { get; set; }
    }
}
