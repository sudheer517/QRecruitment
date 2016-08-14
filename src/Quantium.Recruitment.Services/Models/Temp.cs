using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Quantium.Recruitment.Services.Controllers.TempController;

namespace Quantium.Recruitment.Services.Models
{
    public class Temp
    {
        public int Id { get; set; }

        public string QuestionText { get; set; }

        public IList<TempOption> Options { get; set; }
    }
}
