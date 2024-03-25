using HPCFodmapProject.Server.Data;
using HPCFodmapProject.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HPCFodmapProject.Server.Services;



public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;

    
    //takes in username and ingredientsID and checks if its not whitelsited or fodmap
    //mainly used to help foodintake controller method check if each food is flagged or not
    public bool IsWhiteList(string username, int ingredientsId)
    {
        var userId = (from u in _context.Users
                      where u.UserName == username
                      select u.Id).FirstOrDefault();

        var whiteListItem = (from w in _context.WhiteList
                             where w.UserID == userId && w.IngredientsID == ingredientsId
                             select w).FirstOrDefault();

        return whiteListItem != null && whiteListItem.userIsAffected == 0;
    }
    //Enter a foodId and username , may change implementation to replace foodID with name instead
    //The method checks wether a given food is a fodmap and checks if its on the whitelist
    public bool IsFlaggedFood(string username, int foodId)
    {
        var userId = (from u in _context.Users
                      where u.UserName == username
                      select u.Id).FirstOrDefault();

        var ingredients = (from fi in _context.FoodIngredients
                           join i in _context.Ingredients on fi.IngredientsID equals i.IngredientsID
                           where fi.FoodID == foodId
                           select i.IngredientsID).ToList();

        var whiteListItems = (from w in _context.WhiteList
                              where w.UserID == userId && ingredients.Contains(w.IngredientsID) && w.userIsAffected == 1
                              select w).ToList();

        return whiteListItems.Any();
    }


}
