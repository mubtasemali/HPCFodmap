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

    //should work?
    public async Task<bool> AddFoodIntake(string userName, string foodName, string notes)
    {
        IntakeDto entry = new IntakeDto();
        string passString = $"api/addfoodintake?username?foodname?notes={userName}/{foodName}/{notes}";
       string newString = Uri.EscapeDataString(passString);
        var res = await _httpClient.PostAsJsonAsync(passString, entry);
        if (res.IsSuccessStatusCode)
        {
            return true;
        }
        return false;
    }



}
