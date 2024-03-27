using HPCFodmapProject.Shared;
using System.Net.Http.Json;


namespace HPCFodmapProject.Client.HttpRepository;

public class UserFoodDiaryHttpRepository : IUserFoodDiaryHttpRepository
{
 

    HttpClient _httpClient { get; set; }
    public UserFoodDiaryHttpRepository(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    //private readonly HttpClient _httpClient;
    public async Task<List<IntakeDto>> GetIngredients(string userName)
        {
            List<IntakeDto>? foodDiary = new List<IntakeDto>();
        foodDiary = await _httpClient.GetFromJsonAsync<List<IntakeDto>>($"api/getUserFoodIntake?username={userName}");
        //if (foodDiary == null)
        //{

        //}
        return foodDiary;


        }
       
    
}
