using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using HPCFodmapProject.Shared;
using System.Net.Http.Json;
using HPCFodmapProject.Client.HttpRepository;



namespace HPCFodmapProject.Client.Pages;



public partial class Home
{
    [Inject]
    AuthenticationStateProvider AuthenticationStateProvider { get; set; }
    //new code 
    [Inject]
    UserFoodDiaryHttpRepository UserFoodDiaryHttpRepository  { get; set; }
    [Inject]
    HttpClient Http { get; set; }
    List<IntakeDto> foodDiary = new List<IntakeDto>();
    protected override async Task OnInitializedAsync()

    {

        var UserAuth = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User.Identity;
        if (UserAuth is not null && UserAuth.IsAuthenticated)
        {
            string username = UserAuth.Name;

            //says problem here (THIS DOES NOT WORK)
            foodDiary = await UserFoodDiaryHttpRepository.GetIngredients(UserAuth.Name);


            //COMMENTING OUT TO USE METHOD (THIS METHOD WORKS)

             foodDiary = await Http.GetFromJsonAsync<List<IntakeDto>>("api/getUserFoodIntake?username=" + UserAuth.Name);

        }
    }
}
    
