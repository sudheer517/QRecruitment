using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quantium.Recruitment.Entities
{
    [Table(name: "Test_Label")]
    public class Test_Label : Identifiable
    {
        public virtual Test Test { get; set; }

        public virtual Label Label { get; set; }
    }
}
