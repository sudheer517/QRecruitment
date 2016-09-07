using Reinforced.Typings.Attributes;

namespace Quantium.Recruitment.ApiServices.Models
{
    [TsClass]
    public class CandidateDto
    {
        public long Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public long Mobile { get; set; }

        public bool IsActive { get; set; }

        public long JobId { get; set; }

        public long TestId { get; set; }

        //public JobDto Job { get; set; }

        //public TestDto Test { get; set; }
    }
}
