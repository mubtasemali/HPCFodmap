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
        public string issues { get; set; }

    }
}
