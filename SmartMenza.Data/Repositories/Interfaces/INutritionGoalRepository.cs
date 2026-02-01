using SmartMenza.Data.Entities;

namespace SmartMenza.Data.Repositories.Interfaces
{
    public interface INutritionGoalRepository
    {
        Task<NutricionGoal?> GetLatestByUserIdAsync(int userId);
        Task<NutricionGoal> AddAsync(NutricionGoal goal);
    }
}
