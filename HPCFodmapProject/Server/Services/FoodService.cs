using Microsoft.AspNetCore.Mvc;
using HPCFodmapProject.Shared;
using HPCFodmapProject.Server.Data;
using HPCFodmapProject.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Web;

namespace HPCFodmapProject.Server.Services;

public class FoodService : IFoodService
{
    private readonly ApplicationDbContext _context;

    private readonly UserService _service;

    public FoodService(ApplicationDbContext context)
    {
        _context = context;

        _service = new UserService(context);

    }

    public async Task<bool> updateWhiteList(string username, string IngName)
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
            return true;
        }
        catch (Exception e)
        {
            return false;


        }

    }
    public async Task<bool> AddFoodIntake(string username, string foodName, string notes)
    //limit on notes in query string
    {
        username = HttpUtility.UrlDecode(username);
        foodName = HttpUtility.UrlDecode(foodName);
        notes = HttpUtility.UrlDecode(notes);
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
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }


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
                               harmful = !_service.IsWhiteList(username, i.IngredientsID).Result && (i.inFodMap || _service.IsUserFlagged(username, i.IngredientsID).Result),
                               inFodMap = i.inFodMap
                           }).ToList();

        return ingredients;
    }

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

                           harmful = _service.IsFlaggedFood(username, f.foodName).Result,
                           Food = f.foodName,
                           notes = i.notes,
                           date = i.date,


                       }).ToList();



        return intakes;
    }


    public async Task<bool> DeleteFoodIntake(string username, [FromBody] DeleteIntakeDto intake)
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

            //get  IntakeID with FoodID and Intake.Date
            var IntakeId = (from i in _context.Intake
                            where i.FoodID == foodID
&& i.UserID == userID
&& intake.date == i.date
                            select i.IntakeID).FirstOrDefault();

            var intakeTemp = new Intake { IntakeID = IntakeId, UserID = userID, FoodID = foodID, notes = intake.notes, date = intake.date };
            _context.Intake.Remove(intakeTemp);


            _context.SaveChanges();
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

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

    public async Task<bool> UpdateIntake(string username, IntakeDto intake)
    {
        try { 
        var foodName = intake.Food;


        var foodID = (from f in _context.Food
                      where f.foodName == foodName
                      select f.FoodID).FirstOrDefault();


        var userID = (from u in _context.Users
                      where u.UserName == username
                      select u.Id).FirstOrDefault();
        var IntakeId = (from i in _context.Intake
                        where i.FoodID == foodID
&& i.UserID == userID
&& intake.date == i.date
                        select i.IntakeID).FirstOrDefault();

        var existingIntake = _context.Intake.SingleOrDefault(i => i.FoodID == foodID && i.UserID == userID && i.date == intake.date && i.IntakeID == IntakeId);

        if (existingIntake != null)
        {
            existingIntake.notes = intake.notes;
            _context.SaveChanges();
        }

        return true;
    }
        catch (Exception e)
    {
        return false;
        }
    }


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


    public async Task<bool> UpdateFlagged(string username, string IngName)
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
            return true;
        }
        catch (Exception e)
        {
            return false;

        }

    }



    public async Task<List<WTDto>> GetFodmap()
    {
        var fodmaps = (from i in _context.Ingredients
                       where i.inFodMap == true
                       select new WTDto
                       {
                           ingredient = i.IngredientsName,


                       }).ToList();
        return fodmaps;
    }

    public async Task<bool> AddFodmap(string ingredient)
    {
        try
        {
            var fodmap = new Ingredients { IngredientsName = ingredient, inFodMap = true };
            _context.Ingredients.Add(fodmap);
            _context.SaveChanges();
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    public async Task<bool> DeleteFodmap(string ingredient)
    {
        try
        {
            var fodmap = _context.Ingredients.SingleOrDefault(i => i.IngredientsName == ingredient);
            _context.Ingredients.Remove(fodmap);
            _context.SaveChanges();
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }




}
