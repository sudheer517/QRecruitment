using System.Collections.Generic;

namespace Quantium.Recruitment.Entities
{
    public class Test : Identifiable
    {
        public List<Label> Labels { get; set; }

        public List<Challenge> Challenges { get; set; }

        public List<Candidate> Candidates { get; set; }
    }
}
