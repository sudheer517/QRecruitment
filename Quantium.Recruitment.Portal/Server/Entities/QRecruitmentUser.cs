using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace AspNetCoreSpa.Server.Entities
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class QRecruitmentUser : IdentityUser<int>
    {
        public bool IsEnabled { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; }
        [StringLength(250)]
        public string FirstName { get; set; }
        [StringLength(250)]
        public string LastName { get; set; }

        //Using this only because password sending through email wasnt working. Refactor later
        public string PlainPassword { get; set; }
        [NotMapped]
        public string Name
        {
            get
            {
                return this.FirstName + " " + this.LastName;
            }
        }

    }
}
