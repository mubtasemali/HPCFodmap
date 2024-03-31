using HPCFodmapProject.Shared;



namespace HPCFodmapProject.Client.HttpRepository
{
    public interface IUserHttpRepository
    {
        Task<DataResponse<List<UserEditDto>>> GetAllUsersAsync();
        Task<bool> UpdateUser(UserEditDto user);
        Task<bool> DeleteUser(string userId);
        Task<bool> ToggleAdminUser(string userId);
        Task<bool> ToggleEmailConfirmedUser(string userId);
    }
}
