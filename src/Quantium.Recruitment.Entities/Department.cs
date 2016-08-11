using System.Collections.Generic;

namespace Quantium.Recruitment.Entities
{
    public class Department : Identifiable
    {
        public virtual string Name { get; set; }

        public List<Job> Jobs { get; set; }

        public List<Admin> Admins { get; set; }
    }
}
