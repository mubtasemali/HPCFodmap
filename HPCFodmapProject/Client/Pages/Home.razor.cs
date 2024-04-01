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
    //This user variable is to pass to the AddFoodIntake and other methods
    public string userN = null;
    //variables for ingredients model
    //Creating a list of ingredientsDTOs for when ever user clicks on dialouge box
    List<IngredientsDto> selectedFoodIngredients = new List<IngredientsDto>();
    private bool IsUserModalVisible { get; set; } = false;
    public IntakeDto foodEntry = new IntakeDto();
    //adding dto that we would pass to delete intake method
    IntakeDto deleteIntakeDto = new IntakeDto();
    //adding toast
    public SfToast ToastObj;
    private string? toastContent = string.Empty;
    private string? toastSuccess = "e-toast-success";
    //adding variable to populate checklist for whitelist
    List<WTDto>? whiteLists = new List<WTDto>();
    List<string>? whiteListStrings = new List<string>();


    protected override async Task OnInitializedAsync()
    {
        var UserAuth = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User.Identity;
         string uName = UserAuth.Name;
        this.userN = uName;
        if (UserAuth is not null && UserAuth.IsAuthenticated)
        {
            //calls the gedfoodintake method so we can display food entries in food diary
            foodDiary = await UserFoodDiaryHttpRepository.GetFoodIntake(UserAuth.Name);
            whiteLists = await GetWhiteLists(UserAuth.Name);
        }
    }
    //method Method gets user input for food name and food notes from the front end after the button is clicked 
    //changed method from void so I can call ReloadGrid method
   public async Task getUserInputForEntry(string foodI, string notesI)
    {
        var foodResult = UserFoodDiaryHttpRepository.AddFoodIntake(userN, foodI, notesI);
    if(foodResult != null)
        {
            //need to delay to show most recent change
            await Task.Delay(5000);
            await ReloadGrid();
        }
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
            whiteLists = await GetWhiteLists(userN);
            foreach(var whiteList in whiteLists)
            {
                Console.WriteLine("whitelist:" + whiteList.ingredient);
                whiteListStrings.Add(whiteList.ingredient);
            }
            IsUserModalVisible = true;
        }
        catch
        {
            toastContent = "Error accessing ingredients grid";
            toastSuccess = "e-toast-danger";
            ToastObj.ShowAsync();
        }
    }
    //close method for modol
    public async Task closeIngredientsPopUp()
    {
        IsUserModalVisible = false;
        whiteLists = await GetWhiteLists(userN);
    }
    //adding method for delete and edition functionality to handle click
    public async Task ToolbarClickHandler(ClickEventArgs args)
    {
        if (args.Item.Id == "DeleteEntry")
        {
            if (deleteIntakeDto is not null)
            {
                DeleteIntakeDto returnDeleteDto = new DeleteIntakeDto();
                //assigning values because deleteDto/controller class requires one less variable
                returnDeleteDto.Food = deleteIntakeDto.Food;
                returnDeleteDto.notes = deleteIntakeDto.notes;
                returnDeleteDto.date = deleteIntakeDto.date;
                var isFoodIntakeDeleted = UserFoodDiaryHttpRepository.DeleteFoodIntake(userN, returnDeleteDto);
                //make it refresh after delete
                await Task.Delay(5000);
                await ReloadGrid();
            }
            else
            {
                toastContent = $"Failed to delete user entry";
                toastSuccess = "e-toast-danger";
                StateHasChanged();
                await ToastObj.ShowAsync();
            }

        }
        else
        {
            toastContent = $"Please select an entry";
            toastSuccess = "e-toast-warning";
            StateHasChanged();
            await ToastObj.ShowAsync();
        }
    }
    

   

    //ading another method for delete function: selecting an entry
    public async Task UserRowSelectedHandler(RowSelectEventArgs<IntakeDto> args)
    {
        try
        {
            deleteIntakeDto = args.Data;
        }
        catch
        {
             toastContent = "Error accessing food diary in grid";
             toastSuccess = "e-toast-danger";
             await ToastObj.ShowAsync();
        }
    }

    //adding for toggle whitelist checkbox in ingredients pop up
    public async Task ToggleWhitelist(Microsoft.AspNetCore.Components.ChangeEventArgs args, string ingToUpdate)
    {
        var UserAuth = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User.Identity;
        if (UserAuth is not null && UserAuth.IsAuthenticated)
        {
            await Http.GetAsync($"api/updatewhitelist?username={UserAuth.Name}&IngName={ingToUpdate}");
        }
    }
    //adding for toggle flagged checkbox in ingredients pop up
    public async Task ToggleFlagged(Microsoft.AspNetCore.Components.ChangeEventArgs args, string ingToUpdate)
    {
        var UserAuth = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User.Identity;
        if (UserAuth is not null && UserAuth.IsAuthenticated)
        {
            await Http.GetAsync($"api/updateFlaggedFood?username={UserAuth.Name}&IngName={ingToUpdate}");
        }
    }
    //adding to get whitelist check
    public bool GetWhiteList(string ingredientName)
    {
        bool returnValue = false;
        if (whiteListStrings.Contains(ingredientName))
        {
            returnValue = true;
        }
        Console.WriteLine("name: " + ingredientName);

        
        return returnValue;
    }

    //adding to get whitelist
    public async Task<List<WTDto>> GetWhiteLists(string userName)
    {
        List<WTDto>? whiteLists = new List<WTDto> ();
        whiteLists = await Http.GetFromJsonAsync<List<WTDto>>("api/getWhitelist?username=" + userN);
        foreach (WTDto whitelist in whiteLists)
        {
            Console.WriteLine("Whitelist: " + whitelist.ingredient);
        }
        return whiteLists;


    }
}
    
