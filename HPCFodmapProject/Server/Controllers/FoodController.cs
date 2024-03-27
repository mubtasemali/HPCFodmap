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
using Syncfusion.Blazor.Kanban.Internal;

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
        _service = new UserService(context, userManager);
    }
    //updateWhiteList, it switches the value of the userIsAffected value given a username and ing - > !userIsAffected
    [HttpGet]
    [Route("api/updatewhitelist")]
    public async Task<IActionResult> updateWhiteList(string username, string IngName)
    {

        // var um = await _userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
        try
        {
            var result = (from i in _context.Ingredients
                          where i.IngredientsName == IngName
                          select i.IngredientsID).FirstOrDefault();
            /*            var result = (from u in _context.Users
                                      join w in _context.WhiteList on u.Id equals w.UserID
                                      join i in _context.Ingredients on w.IngredientsID equals i.IngredientsID
                                      where u.UserName == username && i.IngredientsName == IngName
                                      select i.IngredientsID).FirstOrDefault();*/

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
                    whitelistItem.userFlagged = 0;
                }
            }
            else
            {
                var whiteListEntry = new WhiteList { UserID = userID, IngredientsID = result, userIsAffected = 0, userFlagged = 0 };
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

            int? foodID;
            foodID = (from f in _context.Food
                      where f.foodName == foodName//added like
                      select f.FoodID).FirstOrDefault();

            if (foodID == null)
            {

                foodID = (from f in _context.Food
                          where f.foodName.Contains("foodName")//added like
                          select f.FoodID).FirstOrDefault();
            }

            var intake = new Intake { UserID = userID, FoodID = (int)foodID, notes = notes, date = date };
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
    public async Task<List<IngredientsDto>> GetIngredients(string foodName, string username)
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
                               harmful = !_service.IsWhiteList(username, i.IngredientsID).Result && i.inFodMap
                           }).ToList();

        return ingredients;
    }
    //add harmful function for ingredients



    //gets user intake , checks each food for its severity and whitelist status as well, 
    //return List of IntakeDTO, the harmful attribute -> use this to check if food is harmful for specific user
    [HttpGet]
    [Route("api/getUserFoodIntake")]
    public async Task<List<IntakeDto>> GetFoodIntake(string username)
    {
        var userId = (from u in _context.Users
                      where u.UserName == username
                      select u.Id).FirstOrDefault();

        var intakes = (from i in _context.Intake
                       join f in _context.Food on i.FoodID equals f.FoodID
                       where i.UserID == userId
                       select new IntakeDto
                       {

                           harmful = _service.IsFlaggedFood(username, f.FoodID).Result,
                           Food = f.foodName,
                           notes = i.notes,
                           date = i.date,


                       }).ToList();



        return intakes;
    }
    [HttpDelete]
    [Route("api/deleteIntake")]
    public async Task<IActionResult> DeleteFoodIntake(string username, IntakeDto intake)
    //limit on notes in query string
    {


        try
        {
            var foodName = intake.Food;

            var userID = (from u in _context.Users
                          where u.UserName == username
                          select u.Id).FirstOrDefault();
            var foodID = (from f in _context.Food
                          where f.foodName == foodName
                          select f.FoodID).FirstOrDefault();

            var intakeTemp = new Intake { UserID = userID, FoodID = foodID, notes = intake.notes, date = intake.date };
            _context.Intake.Remove(intakeTemp);


            _context.SaveChanges();
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet]
    [Route("api/getFlaggedIntake")]
    public async Task<List<FlaggedDto>> GetFlaggedIntake(string username)
    {
        var userId = (from u in _context.Users
                      where u.UserName == username
                      select u.Id).FirstOrDefault();

        var IntakeFoods = GetFoodIntake(username).Result;
        var flaggedIntake = new List<FlaggedDto>();
        //get all flagged ingredients from IntakeFoods
        foreach (var food in IntakeFoods)
        {
            if (food.harmful)
            {
                var flaggedFood = new FlaggedDto
                {
                    ingredient = food.Food,
                    issues = food.notes,
                    lastEaten = food.date
                };
                flaggedIntake.Add(flaggedFood);
            }
        }
        var flaggedIngredients = new List<FlaggedDto>();
        foreach (var flagged in flaggedIntake)
        {
            //get all ingredient of flagged food
            var ingredients = GetIngredients(flagged.ingredient, username).Result;
            foreach (var ingredient in ingredients)
            {
                if (ingredient.harmful)
                {
                    var flaggedIngredient = new FlaggedDto
                    {
                        ingredient = ingredient.IngredientsName,
                        issues = flagged.issues,
                        lastEaten = flagged.lastEaten
                    };
                    flaggedIngredients.Add(flaggedIngredient);
                }
            }
          
        }



       

        

        return flaggedIngredients;
    }


    //returns list of whitelist items , list of the names of the food
    [HttpGet]
    [Route("api/getWhitelist")]
    public async Task<List<WTDto>> GetWhiteList(string username)
    {


        var userId = (from u in _context.Users
                      where u.UserName == username
                      select u.Id).FirstOrDefault();

        var whiteListedIngredients = (from w in _context.WhiteList
                                      join i in _context.Ingredients on w.IngredientsID equals i.IngredientsID
                                      where w.userIsAffected == 0 && w.UserID == userId && w.userFlagged == 0
                                      select new WTDto
                                      {
                                          ingredient = i.IngredientsName
                                      }
                                      ).Distinct().ToList();

        return whiteListedIngredients;
    }


    [HttpGet]
    [Route("api/updateIntake")]
    public async Task<IActionResult> UpdateIntake(string username, IntakeDto intake)
    {

        var foodName = intake.Food;


        var foodID = (from f in _context.Food
                      where f.foodName == foodName
                      select f.FoodID).FirstOrDefault();


        var userID = (from u in _context.Users
                      where u.UserName == username
                      select u.Id).FirstOrDefault();

        var existingIntake = _context.Intake.SingleOrDefault(i => i.FoodID == foodID && i.UserID == userID && i.date == intake.date);

        if (existingIntake != null)
        {
            existingIntake.notes = intake.notes;
            _context.SaveChanges();
        }

        return Ok();
    }

    //modify this to return a list of ingredients that are flagged
    [HttpGet]
    [Route("api/GetUserFlagged")]
    public async Task<List<FlaggedDto>> GetUserFlagged(string username)
    {
        var userID = (from u in _context.Users
                      where u.UserName == username
                      select u.Id).FirstOrDefault();

        var flaggedIngredients = (from w in _context.WhiteList
                                  join i in _context.Ingredients on w.IngredientsID equals i.IngredientsID
                                  where w.userFlagged == 1 && w.UserID == userID
                                  select new FlaggedDto
                                  {
                                      ingredient = i.IngredientsName,
                                      //issues is the last notes given for the food
                                      issues = (from f in _context.Food
                                                join fi in _context.FoodIngredients on f.FoodID equals fi.FoodID
                                                join ii in _context.Ingredients on fi.IngredientsID equals ii.IngredientsID
                                                join it in _context.Intake on ii.IngredientsID equals it.FoodID
                                                where it.UserID == userID
                                                select it.notes).FirstOrDefault(),
                                      lastEaten = (from f in _context.Food
                                                  join fi in _context.FoodIngredients on f.FoodID equals fi.FoodID
                                                  join ii in _context.Ingredients on fi.IngredientsID equals ii.IngredientsID
                                                  join it in _context.Intake on ii.IngredientsID equals it.FoodID
                                                  where it.UserID == userID
                                                  select it.date).FirstOrDefault()

                                  }).Distinct().ToList();

        return flaggedIngredients;
    }


    [HttpGet]
    [Route("api/updateFlaggedFood")]
    public async Task<IActionResult> UpdateFlagged(string username, string IngName)
    {


        try
        {
            var result = (from i in _context.Ingredients
                          where i.IngredientsName == IngName
                          select i.IngredientsID).FirstOrDefault();


            var userID = (from u in _context.Users
                          where u.UserName == username
                          select u.Id).FirstOrDefault();

            var whitelistItem = (from w in _context.WhiteList
                                 where w.UserID == userID && w.IngredientsID == result
                                 select w).FirstOrDefault();

            if (whitelistItem != null)
            {



                if (whitelistItem.userFlagged == 0)
                {
                    whitelistItem.userFlagged = 1;
                }
                else
                {

                    whitelistItem.userFlagged = 0;
                }
            }
            else
            {
                var whiteListEntry = new WhiteList { UserID = userID, IngredientsID = result, userIsAffected = 1, userFlagged = 1 };
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


    [HttpGet]
    [Route("api/GetFodmap")]

    public async Task<List<WTDto>> GetFodmap()
    {
        var fodmaps = (from i in _context.Ingredients
                       where i.inFodMap == true
                       select new WTDto
                       { 
                           ingredient = i.IngredientsName ,
                           

                       }).ToList();
        return fodmaps;
    }
    [HttpGet]
    [Route("api/AddFodmap")]
    public async Task<IActionResult> AddFodmap(string ingredient)
    {
        try
        {
            var fodmap = new Ingredients { IngredientsName = ingredient, inFodMap = true };
            _context.Ingredients.Add(fodmap);
            _context.SaveChanges();
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete]
    [Route("api/DeleteFodmap")]
    public async Task<IActionResult> DeleteFodmap(string ingredient)
    {
        try
        {
            var fodmap = _context.Ingredients.SingleOrDefault(i => i.IngredientsName == ingredient);
            _context.Ingredients.Remove(fodmap);
            _context.SaveChanges();
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}