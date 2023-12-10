﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Core.QueryFilters
{
    public class PostQueryFilter
    {
        public int? userId {  get; set; }
        public DateTime? date { get; set; }
        public string? description { get; set; }
    }
}
