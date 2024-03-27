namespace HPCFodmapProject.Client.HttpRepository
{
    using HPCFodmapProject.Shared;
    public interface IUserFoodDiaryHttpRepository
    {
        Task<List<IntakeDto>> GetIngredients(string userName);
    }
}
