﻿using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using HPCFodmapProject.Shared;
using System.Net.Http.Json;
using HPCFodmapProject.Client.HttpRepository;
//using System.Net.WebUtility;
using System.Web;




namespace HPCFodmapProject.Client.Pages;



public partial class Home
{
    [Inject]
    AuthenticationStateProvider AuthenticationStateProvider { get; set; }
    [Inject]
    UserFoodDiaryHttpRepository UserFoodDiaryHttpRepository { get; set; }
    [Inject]
    HttpClient Http { get; set; }
    List<IntakeDto> foodDiary = new List<IntakeDto>();
    //IntakeDto newEntry = new IntakeDto();
    //NEW VARIABLES FOR AddFoodIntake
    public string food = null;
    public string notes = null;
    public string userN = null;
    protected override async Task OnInitializedAsync()

    {

        var UserAuth = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User.Identity;
         string uName = UserAuth.Name;
        this.userN = uName;
        if (UserAuth is not null && UserAuth.IsAuthenticated)
        {
            foodDiary = await UserFoodDiaryHttpRepository.GetIngredients(UserAuth.Name);
            //COMMENTING OUT TO USE METHOD (THIS  WORKS)
            //foodDiary = await Http.GetFromJsonAsync<List<IntakeDto>>("api/getUserFoodIntake?username=" + UserAuth.Name);

        }

        //trying something
        //var testing = AddFoodIntake(uName, food, notes);
       
    }
    //adding new method Method gets user input values and assigns them to variables. 
   public void getUserInputForEntry(string foodI, string notesI)
    {
        Console.WriteLine("Passed food sucessfully: " + foodI);
        Console.WriteLine("Passed note sucessfully: " + notesI);
      


     
        var foodResult = AddFoodIntake(userN, foodI, notesI);
        Console.WriteLine("food result: " + foodResult);
    }
    //adding method temporarily 
    public async Task<bool> AddFoodIntake(string userName, string foodName, string notes)
    {
        //making sure it is being passed into method
        Console.WriteLine("Passed food sucessfully into second method: " + foodName);
        Console.WriteLine("Passed note sucessfully into second method: " + notes);
        Console.WriteLine("Passed username sucessfully into second method: " + userName);
        //IntakeDto entry = new IntakeDto();
        //returning a 404
        string passString = $"api/addfoodintake?username={userName}&foodname={foodName}&notes={notes}";
      

     
        //string newString = HttpUtility.UrlEncode(passString);


        var res = await Http.GetFromJsonAsync<bool>(passString);
        //var res = await Http.PostAsJsonAsync($"api/addfoodintake?username={userName}/foodname={foodName}/notes={notes}", entry);
        if (res)
        {
            return true;
        }
        return false;
    }

    //not sure if keeping this method

}
    
