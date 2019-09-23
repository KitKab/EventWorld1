﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMvc.ViewModels
{
    public class PaginationInfo
    {
        public long TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public int ActualPage { get; set; }
        public int TotalPages { get; set; }

        public string Previous { get; set; } // helps put previous tag
        public string Next { get; set; } // helps put next tag
    }
}
