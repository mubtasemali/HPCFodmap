using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPCFodmapProject.Shared
{

    public class WhiteListDto
    {
        public int WhiteListID { get; set; }
        public string UserID { get; set; }
        public int IngredientsID { get; set; }
        //says binary in ERD not sure if this should be bool or not
        public int userIsAffected { get; set; }
    }
}


//fodmaps
//flagged ingredients
//addflag 
//modify intake
