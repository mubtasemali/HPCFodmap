using Microsoft.AspNetCore.Mvc;
using HPCFodmapProject.Shared;
using HPCFodmapProject.Server.Data;
using HPCFodmapProject.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;


namespace HPCFodmapProject.Server.Controllers
{
    public class UserController : Controller
    {
       
        private readonly ApplicationDbContext _context;
        public UserController(ApplicationDbContext context)
        {
            _context = context;

        }

        [HttpGet]
        [Route("api/get-user")]
        public async Task<ActionResult<ApplicationUserDto>> GetUserInfoByUserName(string userName)
        {

            if (userName == null)
            {
               return NotFound();
            }
            if (_context.Users == null)
            {
                return NotFound();
            }

                var user = _context.Users.SingleOrDefault(item => item.UserName == userName);



            var userDto = new ApplicationUserDto
            {
                firstname = user.firstname,
                lastname = user.lastname,
                phoneNumber = user.PhoneNumber
            };

            return userDto;
        }

        

        [HttpPut]
        //[ValidateAntiForgeryToken]
        [Route("api/update-user-info")]
        public async Task <IActionResult> UpdateUserInfo(string userName, ApplicationUserDto User)
        {

            if (userName == null)
            {
                return NotFound();
            }
            if (_context.Users == null)
            {
                return NotFound();
            }

            var user = _context.Users.SingleOrDefault(item => item.UserName == userName);
            user.firstname = User.firstname;
            user.lastname = User.lastname;
            user.PhoneNumber = User.phoneNumber;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }

            return NoContent();
        }


       
    }
}

