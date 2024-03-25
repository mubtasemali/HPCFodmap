using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPCFodmapProject.Shared
{
    public class IngredientsDto
    {
        //public int IngredientsID { get; set; }
        public string IngredientsName { get; set; }
        public int severity { get; set; }
        //says bit in db not sure if this should be bool
        public bool inFodMap { get; set; }
    }
        
}