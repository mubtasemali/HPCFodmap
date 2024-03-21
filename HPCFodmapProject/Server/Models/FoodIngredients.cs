using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HPCFodmapProject.Server.Models
{

    public class FoodIngredients
    {
        //adding key because of: Navigations can only target entity types with keys
        [Key]
        public int FoodIngredientsID { get; set; }
        public int FoodID { get; set; }
        public int IngredientsID { get; set; }

        [ForeignKey("FoodID")]
       [InverseProperty("FoodIngredients")]
        public virtual Food Food { get; set; } = null!;

        [ForeignKey("IngredientsID")]
        [InverseProperty("FoodIngredients")]
        public virtual Ingredients Ingredients { get; set; } = null!;
    }
}
