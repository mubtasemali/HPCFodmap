using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using HPCFodmapProject.Shared;
using System.Net.Http.Json;


namespace HPCFodmapProject.Client.Pages;



public partial class Flagged
{
    [Inject]
    AuthenticationStateProvider AuthenticationStateProvider { get; set; }
    [Inject]
    HttpClient Http { get; set; }
    List<IntakeDto> foodDiary = new List<IntakeDto>();
    protected override async Task OnInitializedAsync()

    {

        var UserAuth = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User.Identity;
        if (UserAuth is not null && UserAuth.IsAuthenticated)
        {
         
            //IntakeDto intakeDto = await Http.GetFromJsonAsync<IntakeDto>("api/getUserFoodIntak?username=" + UserAuth.Name);
            foodDiary = await Http.GetFromJsonAsync<List<IntakeDto>>("api/getUserFoodIntake?username=" + UserAuth.Name);
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
    
