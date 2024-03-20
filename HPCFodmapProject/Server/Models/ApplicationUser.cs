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
        int UserID { get; set; }
        string firstName { get; set; }
        string lastName { get; set; }
        string email { get; set; }
        //might want to handle differently?
        string password { get; set; }
        DateTime registrationDate { get; set; }
        DateTime lastLogin { get; set; }
    }
}