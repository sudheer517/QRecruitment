
namespace Quantium.Recruitment.ApiServices.Models
{
    public class OptionDto
    {
        public long Id { get; set; }

        public long QuestionId { get; set; }

        public string Text { get; set; }

        public bool IsAnswer { get; set; }
    }
}
