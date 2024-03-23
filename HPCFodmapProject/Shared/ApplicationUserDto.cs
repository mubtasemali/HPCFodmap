using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HPCFodmapProject.Shared
{
    public class ApplicationUserDto
    {
        public string? firstname { get; set; } = "temp";
        public string? lastname { get; set; } = "temp";

       public DateTime registrationdate { get; set; }
       public DateTime lastlogin { get; set; }
       public string? phoneNumber { get; set; }

    }
}