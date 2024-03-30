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
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace HPCFodmapProject.Server.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<bool> IsWhiteList(string username, int ingredientsId)
        {
            var userId = (from u in _context.Users
                          where u.UserName == username
                          select u.Id).FirstOrDefault();

            var whiteListItem = (from w in _context.WhiteList
                                 where w.UserID == userId && w.IngredientsID == ingredientsId
                                 select w).FirstOrDefault();

            return (whiteListItem != null) && (whiteListItem.userIsAffected == 0) && (whiteListItem.userFlagged == 0);
        }

        public async Task<bool> IsUserFlagged(string username, int ingredientsId)
        {
            var userId = (from u in _context.Users
                          where u.UserName == username
                          select u.Id).FirstOrDefault();

            var whiteListItem = (from w in _context.WhiteList
                                 where w.UserID == userId && w.IngredientsID == ingredientsId
                                 select w).FirstOrDefault();

            return (whiteListItem != null) && (whiteListItem.userFlagged == 1);
        }

        public async Task<bool> IsFlaggedFood(string username, string food)
        {
            var foodId = (from f in _context.Food
                          where f.foodName == food
                          select f.FoodID).FirstOrDefault();

            var ingredientIds = (from fi in _context.FoodIngredients
                                 where fi.FoodID == foodId
                                 select fi.IngredientsID).ToList();

            var results = new List<(int id, bool IsWhiteListed, bool IsFlagged, bool isFodMap)>();

            foreach (var id in ingredientIds)
            {
                var isWhiteListed = await IsWhiteList(username, id);
                var isFlagged = await IsUserFlagged(username, id);
                var isFodMap = (from i in _context.Ingredients
                                                               where i.IngredientsID == id
                                                                                              select i.inFodMap).FirstOrDefault();  
                results.Add((id, isWhiteListed, isFlagged, isFodMap));
            }

            return results.Any(r => !r.IsWhiteListed && (r.IsFlagged||r.isFodMap));
        }
    }
}
