using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HPCFodmapProject.Server.Models
{
 
    public class Intake
    {
        //adding key because of: Navigations can only target entity types with keys
        [Key]
        public int IntakeID { get; set; }
        public string UserID { get; set; }
        public int FoodID { get; set; }
        public string? notes { get; set; }
        public DateTime date { get; set; } = DateTime.Now;


        [ForeignKey("UserID")]
        [InverseProperty("Intake")]
        public virtual ApplicationUser ApplicationUser { get; set; } = null!;

        [ForeignKey("FoodID")]
        [InverseProperty("Intake")]
        public virtual Food Food { get; set; } = null!;

    }
}
