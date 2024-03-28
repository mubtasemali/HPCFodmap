using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HPCFodmapProject.Server.Models
{
    public class Food
    {
        [Key]
        public int FoodID { get; set; }
        public string foodName { get; set; }
        //adding for new relationship with ingredient table
        [InverseProperty("Food")]
        public virtual ICollection<Ingredients> Ingredients { get; } = new List<Ingredients>();

        [InverseProperty("Food")]
        public virtual ICollection<Intake> Intake { get; } = new List<Intake>();

        [InverseProperty("Food")]
        public virtual ICollection<FoodIngredients> FoodIngredients { get; } = new List<FoodIngredients>();


    }
}
