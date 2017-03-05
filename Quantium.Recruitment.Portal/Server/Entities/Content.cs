using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AspNetCoreSpa.Server.Entities
{
    public class Content : IEntityBase
    {
        [Key]
        public long Id { get; set; }
        [Required]
        [StringLength(250)]
        public string Key { get; set; }

        public ICollection<ContentText> ContentTexts { get; set; }
    }

    public class ContentText
    {
        public long Id { get; set; }
        [Required]
        [StringLength(2048)]
        public string Text { get; set; }
        public virtual Content Content { get; set; }
        public virtual Language Language { get; set; }
        public long ContentId { get; set; }
        public long LanguageId { get; set; }

    }

}

