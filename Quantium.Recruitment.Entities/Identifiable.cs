using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quantium.Recruitment.Entities
{
    public class Identifiable : IDisposable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public virtual long Id { get; set; }

        public void Dispose()
        {
            this.Dispose();
        }
    }
}
