using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quantium.Recruitment.Portal.Server.Entities
{
    public class QRecruitmentRole : IdentityRole<int>
    {
        public QRecruitmentRole() { }
        public QRecruitmentRole(string name)
        {
            this.Name = name;
        }
    }
}
