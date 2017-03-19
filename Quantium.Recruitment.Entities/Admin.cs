using AspNetCoreSpa.Server.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;

namespace Quantium.Recruitment.Entities
{
    [Table(name: "Admin")]
    public class Admin: IEntityBase
    {
        public virtual long Id { get; set; }

        public virtual string FirstName { get; set; }

        public virtual string LastName { get; set; }

        public virtual string Email { get; set; }

        public virtual long Mobile { get; set; }

        public virtual bool IsActive { get; set; }

        public virtual long DepartmentId { get; set; }

        public virtual bool PasswordSent { get; set; }

        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }

        public virtual ICollection<Candidate> Candidates { get; set; }
    }
}
