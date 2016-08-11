using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quantium.Recruitment.Entities
{
    public class Identifiable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public virtual long Id { get; set; }
    }
}
