﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Quantium.Recruitment.Entities;

namespace Quantium.Recruitment.ApiServices.Models
{
    public class JobDto
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public string Profile { get; set; }

        public DepartmentDto Department { get; set; }
    }
}
