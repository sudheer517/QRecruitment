using System.Collections.Generic;
using static Quantium.Recruitment.Services.Controllers.TempController;

namespace Quantium.Recruitment.Services.Models
{
    public class Temp
    {
        public int Id { get; set; }

        public string QuestionText { get; set; }

        public List<TempOption> Options { get; set; }
    }
}
