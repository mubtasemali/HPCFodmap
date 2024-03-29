namespace HPCFodmapProject.Client.HttpRepository
{
    using HPCFodmapProject.Shared;
    public interface IUserFoodDiaryHttpRepository
    {
        Task<List<IntakeDto>> GetFoodIntake(string userName);
        Task<List<IngredientsDto>> GetIngredients(string foodName, string userName);
        Task<bool> AddFoodIntake(string userName, string foodName, string notes);
        Task<bool> DeleteFoodIntake(string userName, DeleteIntakeDto? intake);
    }
}
