namespace HPCFodmapProject.Server.Services
{
    public interface IUserService
    {
        Task<bool> IsFlaggedFood(string username, int foodId);
        Task<bool> IsWhiteList(string username, int ingredientsId);
        Task<bool> IsFlaggedFood(string username, string food);
    }
}
