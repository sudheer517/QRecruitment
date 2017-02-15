using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quantium.Recruitment.Models
{
    public class Candidate_SurveyDto
    {
        public long Id { get; set; }
        public CandidateDto Candidate { get; set; }
        public JobDto Job { get; set; }
        public bool IsFinished { get; set; }

    }
}
