using Microsoft.AspNetCore.Mvc;
using HPCFodmapProject.Shared;
using HPCFodmapProject.Server.Data;
using HPCFodmapProject.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using HPCFodmapProject.Server.Services;

namespace HPCFodmapProject.Server.Controllers;

// call to the server (http://localhost:xxxx/api/user) will call some method in here
// if that call is specific like (http://localhost:xxxx/api/user/) or (http://localhost:xxxx/api/add-movie) calls the appropriate method
public class UserController : Controller
{
    private readonly IUserServiceAdmin _userServiceAdmin;

    public UserController(IUserServiceAdmin userServiceAdmin)
    {
        _userServiceAdmin = userServiceAdmin;
    }

    [HttpGet]
    [Route("api/toggle-admin")]
    [Authorize(Roles = "Admin")]
    public async Task<bool> ToggleAdmin(string userId)
    {
        bool res = await _userServiceAdmin.ToggleAdminService(userId);
        return res;
    }

    //toggle-email-confirmed
    [HttpGet]
    [Route("api/toggle-email-confirmed")]
    [Authorize(Roles = "Admin")]
    public async Task<bool> ToggleEmailConfirmed(string userId)
    {
        bool res = await _userServiceAdmin.ToggleEmailConfirmedService(userId);
        return res;
    }

 

    [HttpPost]
    [Route("api/update-user")]
    [Authorize(Roles = "Admin")]
    public async Task<bool> UpdateUser([FromBody] UserEditDto user)
    {
        var res = await _userServiceAdmin.UpdateUser(user);
        return res;
    }

    //api/delete-user
    [HttpGet]
    [Route("api/delete-user")]
    [Authorize(Roles = "Admin")]
    public async Task<bool> DeleteUser(string userId)
    {
        return await _userServiceAdmin.DeleteUser(userId);
    }

    [HttpGet]
    [Route("api/users")]
    [Authorize(Roles="Admin")]
    public async Task<List<UserEditDto>> GetAllUsers()
    {
        return await _userServiceAdmin.GetAllUsers();
    }

    
}
