﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPCFodmapProject.Shared
{

   
    
      public class FlaggedDto
        {
           
            public string ingredient { get; set; }
            //last notes given in foodintake for given user/food
            public string? issues { get; set; }
            public DateTime lastEaten { get; set; }
        }
    

}
