using HPCFodmapProject.Shared;

using static System.Net.WebRequestMethods;
using System.Net.Http.Json;
using System.Text.Json;
//does not recognize Blazored
//using Blazored.LocalStorage;

namespace HPCFodmapProject.Client.HttpRepository
{
 
    public class UserHttpRepository : IUserHttpRepository
    {
        private readonly HttpClient _httpClient;
        //private readonly ILocalStorageService _localStorageService;
        //removed ILocalStorageService localStorage from method parameter
        public UserHttpRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
            //_localStorageService = localStorage;
        }
        public async Task<bool> UpdateUser(UserEditDto user)
        {
            var res = await _httpClient.PostAsJsonAsync("api/update-user", user);
            if (res.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteUser(string userId)
        {
            var res = await _httpClient.GetFromJsonAsync<bool>($"api/delete-user?userId={userId}");
            return res;
        }


        public async Task<DataResponse<List<UserEditDto>>> GetAllUsersAsync()
        {
            try
            {
                var users = await _httpClient.GetFromJsonAsync<List<UserEditDto>>("api/users");
                return new DataResponse<List<UserEditDto>>()
                {
                    Data = users,
                    Message = "Success",
                    Succeeded = true
                };
            }
            catch (Exception ex)
            {
                // add logging
                return new DataResponse<List<UserEditDto>>()
                {
                    Errors = new Dictionary<string, string[]> { { "Error", new string[] { ex.Message } } },
                    Data = new List<UserEditDto>(),
                    Message = ex.Message,
                    Succeeded = false
                };
            }
        }



        public async Task<bool> ToggleAdminUser(string userId)
        {
            var res = await _httpClient.GetFromJsonAsync<bool>($"api/toggle-admin?userId={userId}");
            return res;
            // add error handling
        }
        public async Task<bool> ToggleEmailConfirmedUser(string userId)
        {
            var res = await _httpClient.GetFromJsonAsync<bool>($"api/toggle-email-confirmed?userId={userId}");
            return res;
            // add error handling
        }
    }
}
