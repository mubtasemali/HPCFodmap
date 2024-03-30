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
    //adding dto that we would pass to delete intake method
    IntakeDto deleteIntakeDto = new IntakeDto();


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
        var foodResult = UserFoodDiaryHttpRepository.AddFoodIntake(userN, foodI, notesI);
    if(foodResult != null)
        {
            //need to delay to show most recent change
            await Task.Delay(5000);
            await ReloadGrid();
        }
        

        Console.WriteLine("food result: " + foodResult);
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

    //adding method for delete and edition functionality to handle click
    public async Task ToolbarClickHandler(ClickEventArgs args)
    {

        //THOUGHTS: I need to get the intakedto for what the user would like to delete
        //DONE!!!! I need to create a method that calls the deleteintake api and takes in intakedto and username
        //i must calle the deleteintake method that i create



        if (args.Item.Id == "DeleteEntry")
        {
            //bool isFoodIntakeDeleted = false;
            if (deleteIntakeDto is not null)
            {
                DeleteIntakeDto returnDeleteDto = new DeleteIntakeDto();
                //assigning values because deleteDto/controller class requires one less variable
                //assigning values because deleteDto/controller class requires one less variable
                returnDeleteDto.Food = deleteIntakeDto.Food;
                returnDeleteDto.notes = deleteIntakeDto.notes;
                returnDeleteDto.date = deleteIntakeDto.date;
                var isFoodIntakeDeleted = UserFoodDiaryHttpRepository.DeleteFoodIntake(userN, returnDeleteDto);
                //make it refresh after delete
                await Task.Delay(5000);
                await ReloadGrid();
            }

            //COMMENTING OUT FOR NOW    
            //    var res = await UserRepo.DeleteUser(userEditDto.Id);
            //    if (res)
            //    {
            //        await ReloadGrid();
            //        toastContent = $"{userEditDto.Email} removed!";
            //        StateHasChanged();
            //        await ToastObj.ShowAsync();
            //    }
            //    else
            //    {
            //        toastContent = $"Failed to delete user {userEditDto.Email}";
            //        toastSuccess = "e-toast-danger";
            //        StateHasChanged();
            //        await ToastObj.ShowAsync();
            //    }

            //}
            //else
            //{
            //    toastContent = $"Please select a user";
            //    toastSuccess = "e-toast-warning";
            //    StateHasChanged();
            //    await ToastObj.ShowAsync();
            //}
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
            /* toastContent = "Error accessing user data in grid";
             toastSuccess = "e-toast-danger";
             await ToastObj.ShowAsync();*/
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
            //updateFlaggedFood
            //await Http.GetFromJsonAsync<>("api/updateFlaggedFood?username=" + UserAuth.Name);
            await Http.GetAsync($"api/updateFlaggedFood?username={UserAuth.Name}&IngName={ingToUpdate}");
        }
    }
}
    
