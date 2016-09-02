namespace Quantium.Recruitment.ApiServices.Models
{
    public class AdminDto: Identifiable
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public long Mobile { get; set; }

        public bool IsActive { get; set; }

        public long DepartmentId { get; set; }

        public DepartmentDto Department { get; set; }
    }
}
