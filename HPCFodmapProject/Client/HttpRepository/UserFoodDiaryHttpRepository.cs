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

    public async Task<bool> AddFoodIntake(string userName, string foodName, string notes)
    {
        string passString = $"api/addfoodintake?username={userName}&foodname={foodName}&notes={notes}";
        var res = await _httpClient.GetFromJsonAsync<bool>(passString);
        if (res)
        {
            return true;
        }
        return false;
    }

    //create method for DeleteFoodIntake
    public async Task<bool> DeleteFoodIntake(string userName, DeleteIntakeDto? intake)
    {
        //for testing
        string food = "did not send food";
        Console.WriteLine("In the controller");
        //if statement is for testing
        if (intake is not null)
        {
            food = intake.Food;
            Console.WriteLine("showing I am getting the food0: " + food);
        }
        string passString = $"api/deleteIntake?username={userName}";
        var res = await _httpClient.PostAsJsonAsync<DeleteIntakeDto>(passString,intake);
        if (res.IsSuccessStatusCode)
        {
            ////make it refresh after delete
            //await Task.Delay(5000);
            //await ReloadGrid();
            return true;
        }
        return false;
    }

    

    }
