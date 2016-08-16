using System.ComponentModel.DataAnnotations.Schema;

namespace Quantium.Recruitment.Entities
{
    [Table(name: "Label")]
    public class Label : Identifiable
    {
        public virtual string Name { get; set; }

        public virtual long JobId { get; set; }

        public virtual long TestId { get; set; }

        [ForeignKey("JobId")]
        public virtual Job Job { get; set; }

        [ForeignKey("TestId")]
        public virtual Test Test { get; set; }
    }
}
