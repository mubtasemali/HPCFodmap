using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPCFodmapProject.Shared
{
 
    public class IntakeDto
    {
        public int IntakeID { get; set; }
        public string UserID { get; set; }
        public int FoodID { get; set; }
        public string notes { get; set; }
        public DateTime date { get; set; } = DateTime.Now;
    }
}
