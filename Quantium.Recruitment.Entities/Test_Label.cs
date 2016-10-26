﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quantium.Recruitment.Entities
{
    [Table(name: "Test_Label")]
    public class Test_Label : Identifiable
    {
        public virtual long TestId { get; set; }

        public virtual long LabelId { get; set; }

        [ForeignKey("TestId")]
        public virtual Test Test { get; set; }

        [ForeignKey("LabelId")]
        public virtual Label Label { get; set; }
    }
}
