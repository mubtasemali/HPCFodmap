﻿using Syncfusion.Blazor.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPCFodmapProject.Shared
{

    public class DeleteIntakeDto
    {
        public string Food { get; set; }
        public string? notes { get; set; }
        public DateTime date { get; set; } = DateTime.Now;


    }
}