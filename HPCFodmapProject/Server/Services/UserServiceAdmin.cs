using HPCFodmapProject.Shared;
using HPCFodmapProject.Server.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Duende.IdentityServer.Validation;
using HPCFodmapProject.Server.Models;


namespace HPCFodmapProject.Server.Services;
public class UserServiceAdmin : IUserServiceAdmin
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<UserService> _logger;
    private readonly UserManager<ApplicationUser> _userManager;

    public UserServiceAdmin(ApplicationDbContext context, ILogger<UserService> logger, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _logger = logger;
        _userManager = userManager;
    }

    

    public async Task<bool> DeleteUser(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is not null)
        {
            var res = await _userManager.DeleteAsync(user);
            return res.Succeeded;
        }
        return false;
    }

    public async Task<List<UserEditDto>> GetAllUsers()
    {
        var users = (from u in _context.Users
                     let query = (from ur in _context.Set<IdentityUserRole<string>>()
                                  where ur.UserId.Equals(u.Id)
                                  join r in _context.Roles on ur.RoleId equals r.Id
                                  select r.Name).ToList()
                     select new UserEditDto
                     {
                         Id = u.Id,
                         UserName = u.UserName,
                         Email = u.Email,
                         FirstName = u.firstname,
                         LastName = u.lastname,
                         EmailConfirmed = u.EmailConfirmed,
                         Admin = query.Contains("Admin")
                     }).ToList();
        _logger.LogInformation("retrieving all users.  Logged at {Placeholder:MMMM dd, yyyy}",
                       DateTimeOffset.UtcNow);

        return users;

    }
    public async Task<bool> UpdateUser(UserEditDto user)
    {
        var userToUpdate = await _userManager.FindByIdAsync(user.Id);
        if (userToUpdate is null)
        {
            return false;
        }
        if (user.EmailConfirmed != userToUpdate.EmailConfirmed)
        {
            userToUpdate.EmailConfirmed = user.EmailConfirmed;
        }
        var roles = await _userManager.GetRolesAsync(userToUpdate);
        if (!user.Admin && roles.Contains("Admin"))
        {
            await _userManager.RemoveFromRoleAsync(userToUpdate, "Admin");
        }
        else if (user.Admin && !roles.Contains("Admin"))
        {
            await _userManager.AddToRoleAsync(userToUpdate, "Admin");
        }
        userToUpdate.UserName = user.UserName;
        userToUpdate.Email = user.Email;
        userToUpdate.firstname = user.FirstName;
        userToUpdate.lastname = user.LastName;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ToggleAdminService(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return false;
        }
        var roles = await _userManager.GetRolesAsync(user);
        if (roles.Contains("Admin"))
        {
            await _userManager.RemoveFromRoleAsync(user, "Admin");
        }
        else
        {
            await _userManager.AddToRoleAsync(user, "Admin");
        }
        return true;
    }

    public async Task<bool> ToggleEmailConfirmedService(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return false;
        }
        user.EmailConfirmed = !user.EmailConfirmed;
        await _context.SaveChangesAsync();
        return true;
    }
}
