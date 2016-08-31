using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quantium.Recruitment.Entities
{
    //[Table(name: "Department")]
    public class Department: Identifiable
    {
        public virtual string Name { get; set; }

        //public virtual List<Job> Jobs { get; set; }

        //public virtual List<Admin> Admins { get; set; }
    }
}
