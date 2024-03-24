using Microsoft.AspNetCore.Identity;
//added below
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
namespace HPCFodmapProject.Server.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? firstname { get; set; } = "firstName";
        public string? lastname { get; set; } = "lastName";

       public DateTime registrationdate { get; set; }
       public DateTime lastlogin { get; set; }

        [InverseProperty("ApplicationUser")]
        public virtual ICollection<Intake> Intake { get; } = new List<Intake>();

        [InverseProperty("ApplicationUser")]
        public virtual ICollection<WhiteList> WhiteLists { get; } = new List<WhiteList>();
    }
}