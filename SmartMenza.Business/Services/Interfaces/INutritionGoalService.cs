using SmartMenza.Business.Models;

namespace SmartMenza.Business.Services
{
    public interface INutritionGoalService
    {
        Task<NutritionGoalResponse> GetMyGoalAsync(int userId);
        Task<NutritionGoalResponse> SetMyGoalAsync(int userId, SetNutritionGoalRequest request);
    }
}
