
namespace Quantium.Recruitment.Models
{
    public class OptionDto
    {
        public long Id { get; set; }

        public long QuestionId { get; set; }

        public string Text { get; set; }

        public string ImageUrl { get; set; }

        public bool IsAnswer { get; set; }

        public bool IsCandidateSelected { get; set; }
    }
}
