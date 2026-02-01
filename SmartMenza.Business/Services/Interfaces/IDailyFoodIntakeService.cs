using SmartMenza.Business.Models;

namespace SmartMenza.Business.Services.Interfaces
{
    public interface IDailyFoodIntakeService
    {
        Task<List<DailyFoodIntakeResponse>> GetMyTodayAsync(int userId);
        Task<DailyFoodIntakeResponse> AddToMyTodayAsync(int userId, AddDailyFoodIntakeRequest request);
        Task RemoveFromMyTodayAsync(int userId, int dailyFoodIntakeId);
    }
}
