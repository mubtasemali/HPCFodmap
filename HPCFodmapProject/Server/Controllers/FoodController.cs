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

using HPCFodmapProject.Server.Services;

//NEED THIS FOR DECODING string route in the addfood controller 
using System.Web;

namespace HPCFodmapProject.Server.Controllers;

public class FoodController : Controller
{
    private readonly IFoodService _service;
   
    public FoodController(IFoodService foodService)
    {
        _service = foodService;
    }
    //updateWhiteList, it switches the value of the userIsAffected value given a username and ing - > !userIsAffected
    [HttpGet]
    [Route("api/updatewhitelist")]
    public async Task<bool> updateWhiteList(string username, string IngName)
    {
        var result = await _service.updateWhiteList(username, IngName);
        return result;

    }
    //adds foodIntake from front end and updates database 
    [HttpGet]
    [Route("api/addfoodintake")]

    public async Task<bool> AddFoodIntake(string username, string foodName, string notes)
    //limit on notes in query string
    {
      
        var result = await _service.AddFoodIntake(username, foodName, notes);
        return result;
            
    }
    //returns a list of Ingredients information in the form of a IngredientDTO for a given food
    [HttpGet]
    [Route("api/getIngredients")]
    public async Task<List<IngredientsDto>> GetIngredients(string foodName, string username)
    {
        var ingredients = await _service.GetIngredients(foodName, username);
        return ingredients;
    }
    //add harmful function for ingredients



    //gets user intake , checks each food for its severity and whitelist status as well, 
    //return List of IntakeDTO, the harmful attribute -> use this to check if food is harmful for specific user
    [HttpGet]
    [Route("api/getUserFoodIntake")]
    public async Task<List<IntakeDto>> GetFoodIntake(string username)
    {
        var intake = await _service.GetFoodIntake(username);
        return intake;
    }

    [HttpPost]
    [Route("api/deleteIntake")]
    public async Task<bool> DeleteFoodIntake(string username, [FromBody] DeleteIntakeDto intake)
    //limit on notes in query string
    {


        var result = await _service.DeleteFoodIntake(username, intake);
        return result;
    }


    //returns list of whitelist items , list of the names of the food
    [HttpGet]
    [Route("api/getWhitelist")]
    public async Task<List<WTDto>> GetWhiteList(string username)
    {


        var whitelist = await _service.GetWhiteList(username);
        return whitelist;
    }


    [HttpGet]
    [Route("api/updateIntake")]
    public async Task<bool> UpdateIntake(string username, IntakeDto intake)
    {
        var result = await _service.UpdateIntake(username, intake);
        return result;
    }

    //modify this to return a list of ingredients that are flagged
    [HttpGet]
    [Route("api/GetUserFlagged")]
    public async Task<List<FlaggedDto>> GetUserFlagged(string username)
    {
        var flagged = await _service.GetUserFlagged(username);
        return flagged;
    }


    [HttpGet]
    [Route("api/updateFlaggedFood")]
    public async Task<bool> UpdateFlagged(string username, string IngName)
    {

        var result = await _service.UpdateFlagged(username, IngName);
        return result;

    }


    [HttpGet]
    [Route("api/GetFodmap")]

    public async Task<List<WTDto>> GetFodmap()
    {
        var fodmap = await _service.GetFodmap();
        return fodmap;
    }
    [HttpGet]
    [Route("api/AddFodmap")]
    public async Task<bool> AddFodmap(string ingredient)
    {
          var result = await _service.AddFodmap(ingredient);
        return result;
    }

    [HttpDelete]
    [Route("api/DeleteFodmap")]
    public async Task<bool> DeleteFodmap(string ingredient)
    {
        var result = await _service.DeleteFodmap(ingredient);
        return result;
    }
}