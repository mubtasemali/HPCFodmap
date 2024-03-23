using Microsoft.AspNetCore.Mvc;
using HPCFodmapProject.Shared;
using HPCFodmapProject.Server.Data;
using HPCFodmapProject.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
//added
using AutoMapper;
namespace HPCFodmapProject.Server.Controllers
{
    public class UserController : Controller
    {
        //pasted code
        private readonly ApplicationDbContext _context;
        public UserController(ApplicationDbContext context)
        {
            _context = context;

        }

        [HttpGet]
        [Route("api/user")]
        public async Task<ApplicationUserDto> GetUserInfoByUserName(string userName)
        {
            //sends error message back if no userName is entered
            if (userName == null)
            {
                //setting value just to test
                userName = "maiq@cat.com";
                //return NotFound(new {Messgae = ""});
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

        

       

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        //// GET: UserController/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: UserController/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}

