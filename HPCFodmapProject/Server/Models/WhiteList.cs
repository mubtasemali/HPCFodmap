﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HPCFodmapProject.Server.Models
{
    public class WhiteList
    {
        public int UserID { get; set; }
        public int IngredientsID { get; set; }
        //says binary in ERD not sure if this should be bool or not
        public int userIsAffected { get; set; }

        [ForeignKey("UserID")]
        [InverseProperty("WhiteLists")]
        public virtual ApplicationUser ApplicationUser { get; set; } = null!;

        [ForeignKey("IngredientsID")]
        [InverseProperty("WhiteLists")]
        public virtual Ingredients Ingredients { get; set; } = null!;
    }
}
