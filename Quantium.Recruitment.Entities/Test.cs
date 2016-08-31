using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quantium.Recruitment.Entities
{
    [Table(name: "Test")]
    public class Test : Identifiable
    {
        public virtual string Name { get; set; }

        public virtual List<Label> Labels { get; set; }

        //public virtual List<Challenge> Challenges { get; set; }

        public virtual List<Candidate> Candidates { get; set; }
    }
}
