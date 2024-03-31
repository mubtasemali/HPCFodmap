using HPCFodmapProject.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IFoodService
{
    Task<bool> updateWhiteList(string username, string IngName);
    Task<bool> AddFoodIntake(string username, string foodName, string notes);
    Task<List<IngredientsDto>> GetIngredients(string foodName, string username);
    Task<List<IntakeDto>> GetFoodIntake(string username);
    Task<bool> DeleteFoodIntake(string username, DeleteIntakeDto intake);
    Task<List<WTDto>> GetWhiteList(string username);
    Task<bool> UpdateIntake(string username, IntakeDto intake);
    Task<List<FlaggedDto>> GetUserFlagged(string username);
    Task<bool> UpdateFlagged(string username, string IngName);
    Task<List<WTDto>> GetFodmap();
    Task<bool> AddFodmap(string ingredient);
    Task<bool> DeleteFodmap(string ingredient);
}