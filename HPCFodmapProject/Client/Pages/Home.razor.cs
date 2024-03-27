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

            //IntakeDto intakeDto = await Http.GetFromJsonAsync<IntakeDto>("api/getUserFoodIntak?username=" + UserAuth.Name);
            string username = UserAuth.Name;

            //says problem here 
            foodDiary = await UserFoodDiaryHttpRepository.GetIngredients(username);



            //COMMENTING OUT TO USE METHOD

            // foodDiary = await Http.GetFromJsonAsync<List<IntakeDto>>("api/getUserFoodIntake?username=" + UserAuth.Name);











            //var foodDiary = await HttpClient.GetAsync<List<IntakeDto>>("/api/cars");
            //if (foodDiary?.Any() ?? false)
            //{
            //    foreach(var food in foodDiary)
            //    {
            //        IntakeDto intake = await Http.GetFromJsonAsync<IntakeDto>(food.ToString());
            //    }
        }
    }
}
    
