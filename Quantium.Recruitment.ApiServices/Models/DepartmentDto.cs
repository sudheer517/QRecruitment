using System.Collections.Generic;

namespace Quantium.Recruitment.ApiServices.Models
{
    public class DepartmentDto: Identifiable
    {
        public string Name { get; set; }

        public List<JobDto> Jobs { get; set; }

        public List<AdminDto> Admins { get; set; }
    }
}
