using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HPCFodmapProject.Server.Models
{
    public class Ingredients
    {
        [Key]
        public int IngredientsID { get; set; }
        //added food id for new relationship
        public int FoodID { get; set; }
        public string IngredientsName { get; set; }
        public int severity { get; set; }
        //says bit in db not sure if this should be bool
        //changed back to resolve issues 
        public bool inFodMap { get; set; }

        [InverseProperty("Ingredients")]
        public virtual ICollection<WhiteList> WhiteLists { get; } = new List<WhiteList>();

        [InverseProperty("Ingredients")]
        public virtual ICollection<FoodIngredients> FoodIngredients { get; } = new List<FoodIngredients>();

    }
        
}