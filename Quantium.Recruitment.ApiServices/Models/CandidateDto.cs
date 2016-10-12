namespace Quantium.Recruitment.ApiServices.Models
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

        public string College { get; set; }
    }
}
