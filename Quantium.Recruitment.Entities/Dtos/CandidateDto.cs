using System;
using System.Collections.Generic;

namespace Quantium.Recruitment.Models
{
    public class CandidateDto
    {
        public long Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public long Mobile { get; set; }

        public bool IsActive { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Country { get; set; }

        public string Branch { get; set; }

        public string College { get; set; }

        public string PassingYear { get; set; }

        public double CGPA { get; set; }

        public double ExperienceInYears { get; set; }

        public string CurrentCompany { get; set; }

        public bool IsInformationFilled { get; set; }

        public bool PasswordSent { get; set; }

        public int TestMailSent { get; set; }

        public DateTime? CreatedUtc { get; set; }

        public string Password { get; set; }
    }
}
