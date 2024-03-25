using Microsoft.AspNetCore.Mvc;
using HPCFodmapProject.Shared;
using HPCFodmapProject.Server.Data;
using HPCFodmapProject.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;


using HPCFodmapProject.Server.Models;
using HPCFodmapProject.Server.Data.Migrations;
using Syncfusion.Blazor.Diagram;
using HPCFodmapProject.Server.Services;
using Syncfusion.Blazor.Notifications;

namespace HPCFodmapProject.Server.Controllers;

public class FoodController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly UserService _service;
    public FoodController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    { 
        _context = context;
        _userManager = userManager;
        _service = new UserService();
    }
    //updateWhiteList, it switches the value of the userIsAffected value given a username and ing - > !userIsAffected
    [HttpGet]
    [Route("api/updatewhitelist")]
    public async Task<IActionResult> updateWhiteList(string username, string IngName)
    {

        // var um = await _userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
        try
        {
            var result = (from u in _context.Users
                          join w in _context.WhiteList on u.Id equals w.UserID
                          join i in _context.Ingredients on w.IngredientsID equals i.IngredientsID
                          where u.UserName == username && i.IngredientsName == IngName
                          select i.IngredientsID).FirstOrDefault();

            var userID = (from u in _context.Users
                          where u.UserName == username
                          select u.Id).FirstOrDefault();

            var whitelistItem = (from w in _context.WhiteList
                                 where w.UserID == userID && w.IngredientsID == result
                                 select w).FirstOrDefault();
            if (whitelistItem != null)
            {
                if (whitelistItem.userIsAffected == 0)
                {
                    whitelistItem.userIsAffected = 1;
                }
                else
                {
                    whitelistItem.userIsAffected = 0;
                }
            }
            else
            {
                var whiteListEntry = new WhiteList { UserID = userID, IngredientsID = result, userIsAffected = 1 };
                _context.WhiteList.Add(whiteListEntry);
            }
            
            _context.SaveChanges();
            return Ok();
        }
        catch (Exception e)
        {
            return NotFound(e.Message);


        }

    }
    //adds foodIntake from front end and updates database 
    [HttpGet]
    [Route("api/addfoodintake")]

    public async Task<IActionResult> AddFoodIntake(string username, string foodName, string notes)
        //limit on notes in query string
    {

        try
        {
            DateTime date = DateTime.Now;
            var userID = (from u in _context.Users
                          where u.UserName == username
                          select u.Id).FirstOrDefault();
            var foodID = (from f in _context.Food
                          where f.foodName == foodName
                          select f.FoodID).FirstOrDefault();

            var intake = new Intake { UserID = userID, FoodID = foodID, notes = notes, date = date };
            _context.Intake.Add(intake);


            _context.SaveChanges();
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    //returns a list of Ingredients information in the form of a IngredientDTO for a given food
    [HttpGet]
    [Route("api/getIngredients")]
    public async Task<List<IngredientsDto>> GetIngredients (string foodName, string username)
    {
        var userID = (from u in _context.Users
                      where u.UserName == username
                      select u.Id).FirstOrDefault();

        var foodId = (from f in _context.Food
                      where f.foodName == foodName
                      select f.FoodID).FirstOrDefault();

        var ingredients = (from fi in _context.FoodIngredients
                           join i in _context.Ingredients on fi.IngredientsID equals i.IngredientsID
                           where fi.FoodID == foodId
                           select new IngredientsDto
                           {

                               IngredientsName = i.IngredientsName,
                               harmful = !_service.IsWhiteList(username, i.IngredientsID) && i.inFodMap
                           }).ToList();

        return ingredients;
    }
 //add harmful function for ingredients



    //gets user intake , checks each food for its severity and whitelist status as well, 
    //return List of IntakeDTO, the harmful attribute -> use this to check if food is harmful for specific user
    [HttpGet]
    [Route("api/getUserFoodIntake")]
    public async  Task <List<IntakeDto>> GetFoodIntake(string username)
    {
        var userId = (from u in _context.Users
                      where u.UserName == username
                      select u.Id).FirstOrDefault();

        var intakes = (from i in _context.Intake
                       join f in _context.Food on i.FoodID equals f.FoodID
                       where i.UserID == userId
                       select new IntakeDto
                       {
                           
                           harmful = _service.IsFlaggedFood(username, f.FoodID),
                           Food = f.foodName,
                           notes = i.notes,
                           date = i.date,


                       }).ToList();



        return intakes;
    }

    [HttpGet]
    [Route("api/getFlaggedFoods")]
    public async Task<List<FlaggedFoodDto>> GetFlaggedFoods(string username)
    {
        var userId = (from u in _context.Users
                      where u.UserName == username
                      select u.Id).FirstOrDefault();

        var flaggedFoods = (from i in _context.Intake
                            join f in _context.Food on i.FoodID equals f.FoodID
                            join fi in _context.FoodIngredients on f.FoodID equals fi.FoodID
                            join ing in _context.Ingredients on fi.IngredientsID equals ing.IngredientsID
                            join w in _context.WhiteList on  ing.IngredientsID equals w.IngredientsID
                            where w.userIsAffected == 1 && w.UserID == userId
                            select new FlaggedFoodDto
                            {
                                foodName = f.foodName,
                                issues = i.notes,
                                lastEaten = i.date
                            }).ToList();

        return flaggedFoods;



    }

    
    //returns list of whitelist items , list of the names of the food
    [HttpGet]
    [Route("api/getWhitelist")]
    public async Task<List<string>> GetWhiteList(string username)
    {
        var userId = (from u in _context.Users
                      where u.UserName == username
                      select u.Id).FirstOrDefault();

        var whiteListFoods = (from i in _context.Intake
                              join w in _context.WhiteList on i.FoodID equals w.IngredientsID
                              where w.userIsAffected == 0 && w.UserID == userId
                              select i.Food.foodName).Distinct().ToList();

        return whiteListFoods;

    }



}






