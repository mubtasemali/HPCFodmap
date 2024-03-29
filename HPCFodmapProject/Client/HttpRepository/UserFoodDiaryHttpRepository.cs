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
    public async Task<List<IntakeDto>> GetFoodIntake(string userName)
        {
            List<IntakeDto>? foodDiary = new List<IntakeDto>();
        foodDiary = await _httpClient.GetFromJsonAsync<List<IntakeDto>>($"api/getUserFoodIntake?username={userName}");
        //if (foodDiary == null)
        //{

        //}
        return foodDiary;


        }
    public async Task<List<IngredientsDto>> GetIngredients(string foodName, string userName)
    {
        List<IngredientsDto>? selectedFoodIngredients = new List<IngredientsDto>();
        selectedFoodIngredients = await _httpClient.GetFromJsonAsync<List<IngredientsDto>>($"api/getIngredients?foodName={foodName}&username={userName}");
        //if (foodDiary == null)
        //{

        //}
        return selectedFoodIngredients;
    }




}
