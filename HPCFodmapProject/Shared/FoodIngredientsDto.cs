using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPCFodmapProject.Shared
{

    public class FoodIngredientsDto
    {
        //adding key because of: Navigations can only target entity types with keys
        public int FoodIngredientsID { get; set; }
        public int FoodID { get; set; }
        public int IngredientsID { get; set; }
    }
}
