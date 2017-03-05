using AspNetCoreSpa.Server.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quantium.Recruitment.Entities
{
    [Table(name: "Department")]
    public class Department: IEntityBase
    {
        public virtual long Id { get; set; }
        public virtual string Name { get; set; }

        public virtual ICollection<Job> Jobs { get; set; }

        public virtual ICollection<Admin> Admins { get; set; }
    }
}
