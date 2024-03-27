using HPCFodmapProject.Shared;
using System.Net.Http.Json;


namespace HPCFodmapProject.Client.HttpRepository;

public class UserFoodDiaryHttpRepository
{
  

        //HttpClient Http { get; set; }
        private readonly HttpClient _httpClient;
        public async Task<List<IntakeDto>> GetIngredients(string userName)
        {
            List<IntakeDto>? foodDiary = new List<IntakeDto>();
        //says problem with below 
            foodDiary = await _httpClient.GetFromJsonAsync<List<IntakeDto>>("api/getUserFoodIntake?username=" + userName);
            //if (foodDiary == null)
            //{
               
            //}
            return foodDiary;


        }
       
    
}
