using System.Collections.Generic;
using Reinforced.Typings.Attributes;

namespace Quantium.Recruitment.ApiServices.Models
{
    [TsClass]
    public class DepartmentDto: Identifiable
    {
        public string Name { get; set; }

        public List<JobDto> Jobs { get; set; }

        public List<AdminDto> Admins { get; set; }
    }
}
