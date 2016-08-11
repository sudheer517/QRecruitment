using System.ComponentModel.DataAnnotations.Schema;

namespace Quantium.Recruitment.Entities
{
    public class Label : Identifiable
    {
        public virtual string Name { get; set; }

        public virtual long JobId { get; set; }

        public virtual long TestId { get; set; }

        [ForeignKey("JobId")]
        public Job Job { get; set; }

        [ForeignKey("TestId")]
        public Test Test { get; set; }
    }
}
