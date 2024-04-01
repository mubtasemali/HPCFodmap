using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPCFodmapProject.Shared;

public class UserEditDto
{
    public string Id { get; set; }

    public string UserName { get; set; }
    [EmailAddress]
    [Required]
    [MinLength(3)]
    public string Email { get; set; }
    [MinLength(3)]
    public string FirstName { get; set; }
    [MinLength(3)]
    public string LastName { get; set; }
    public bool EmailConfirmed { get; set; }
    public bool Admin { get; set; }

}