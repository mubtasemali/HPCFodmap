
using HPCFodmapProject.Shared;

namespace HPCFodmapProject.Server.Services
{
    public interface IUserServiceAdmin
    {
        Task<List<UserEditDto>> GetAllUsers();
        Task<bool> UpdateUser(UserEditDto user);
        Task<bool> DeleteUser(string userId);
        Task<bool> ToggleAdminService(string userId);
        Task<bool> ToggleEmailConfirmedService(string userId);
    }
}
