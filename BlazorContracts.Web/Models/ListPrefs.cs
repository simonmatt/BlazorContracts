﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorContracts.Web.Models
{
    public class ListPrefs
    {
        public string OrderBy { get; set; } = "Id";
        public int NumResults { get; set; } = 10;
    }
}
