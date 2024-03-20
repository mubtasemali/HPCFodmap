using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HPCFodmapProject.Server.Models
{
    public class Intake
    {
        public int UserID { get; set; }
        public int FoodID { get; set; }
        public string notes { get; set; }
        public DateTime date { get; set; } = DateTime.Now;


        [ForeignKey("UserID")]
        [InverseProperty("Intakes")]
        public virtual ApplicationUser ApplicationUser { get; set; } = null!;

        [ForeignKey("FoodID")]
        [InverseProperty("Intakes")]
        public virtual Food Food { get; set; } = null!;

    }
}
