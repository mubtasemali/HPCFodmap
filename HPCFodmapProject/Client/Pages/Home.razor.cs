using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using HPCFodmapProject.Shared;
using System.Net.Http.Json;
using HPCFodmapProject.Client.HttpRepository;
//using System.Net.WebUtility;
using System.Web;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Notifications;
using Syncfusion.Blazor.Navigations;
using static System.Net.WebRequestMethods;

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
    //VARIABLES FOR AddFoodIntake
    public string food = null;
    public string notes = null;
    //This user variable is to pass to the AddFoodIntake method
    public string userN = null;

    //variables for ingredients model
    //Creating a list of ingredientsDTOs for when ever user clicks on dialouge box
    List<IngredientsDto> selectedFoodIngredients = new List<IngredientsDto>();
    private bool IsUserModalVisible { get; set; } = false;
    public IntakeDto foodEntry = new IntakeDto();


    protected override async Task OnInitializedAsync()

    {

        var UserAuth = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User.Identity;
         string uName = UserAuth.Name;
        this.userN = uName;
        if (UserAuth is not null && UserAuth.IsAuthenticated)
        {
            //calls the gedfoodintake method so we can display food entries in food diary
            foodDiary = await UserFoodDiaryHttpRepository.GetFoodIntake(UserAuth.Name);
        }
       
    }
    //method Method gets user input for food name and food notes from the front end after the button is clicked 
    //changed method from void so I can call ReloadGrid method
   public async Task getUserInputForEntry(string foodI, string notesI)
    {
        var foodResult = AddFoodIntake(userN, foodI, notesI);
    if(foodResult != null)
        {
            //need to delay to show most recent change
            await Task.Delay(5000);
            await ReloadGrid();
        }
        

        Console.WriteLine("food result: " + foodResult);
    }
    // 
    public async Task<bool> AddFoodIntake(string userName, string foodName, string notes)
    {
        string passString = $"api/addfoodintake?username={userName}&foodname={foodName}&notes={notes}";
        var res = await Http.GetFromJsonAsync<bool>(passString);
        if (res)
        {
            return true;
        }
        return false;
    }
//method for refreshing the page 
    public async Task ReloadGrid()
    {
        var UserAuth = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User.Identity;
        string uName = UserAuth.Name;
        this.userN = uName;
        if (UserAuth is not null && UserAuth.IsAuthenticated)
        {
            foodDiary = await UserFoodDiaryHttpRepository.GetFoodIntake(UserAuth.Name);
        }
    }


    //method for getting ingredients popup
    public async Task UserDoubleClickHandler(RecordDoubleClickEventArgs<IntakeDto> args)
    {
        try
        {
            foodEntry = args.RowData;
            string foodN = foodEntry.Food;
            selectedFoodIngredients = await UserFoodDiaryHttpRepository.GetIngredients(foodN,userN).ConfigureAwait(false);
            IsUserModalVisible = true;
        }
        catch
        {
            //toastContent = "Error accessing user data in grid";
            //toastSuccess = "e-toast-danger";
            //await ToastObj.ShowAsync();
        }
    }
    //close method for modol
    public async Task closeIngredientsPopUp()
    {
        IsUserModalVisible = false;
    }
}
    
