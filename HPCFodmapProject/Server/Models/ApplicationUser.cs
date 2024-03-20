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
        [Key]
        int userid { get; set; }
        string firstname { get; set; }
        string lastname { get; set; }
        string email { get; set; }
        //might want to handle differently?
        string password { get; set; }
        DateTime registrationdate { get; set; }
        DateTime lastlogin { get; set; }
    }
}