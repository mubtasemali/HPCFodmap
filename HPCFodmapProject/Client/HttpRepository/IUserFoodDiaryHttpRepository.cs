namespace HPCFodmapProject.Client.HttpRepository
{
    using HPCFodmapProject.Shared;
    public interface IUserFoodDiaryHttpRepository
    {
        Task<List<IntakeDto>> GetFoodIntake(string userName);
    }
}
