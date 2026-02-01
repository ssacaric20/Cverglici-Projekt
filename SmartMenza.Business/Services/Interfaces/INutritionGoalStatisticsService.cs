using SmartMenza.Business.Models;

namespace SmartMenza.Business.Services.Interfaces
{
    public interface INutritionGoalStatisticsService
    {
        Task<TodayNutritionProgressResponse> GetTodayProgressAsync(int userId);
    }
}
