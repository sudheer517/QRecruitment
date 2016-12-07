using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Quantium.Recruitment.Entities;

namespace Quantium.Recruitment.Models
{
    public class Candidate_JobDto
    {
        public long Id { get; set; }

        public CandidateDto Candidate { get; set; }

        public JobDto Job { get; set; }

        public bool IsFinished { get; set; }
    }
}
