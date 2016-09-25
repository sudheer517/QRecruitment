using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Quantium.Recruitment.Portal.Models
{
    public class QRecruitmentRole : IdentityRole
    {
        public QRecruitmentRole() { }
        public QRecruitmentRole(string name)
        {
            this.Name = name;
        }
    }
}
