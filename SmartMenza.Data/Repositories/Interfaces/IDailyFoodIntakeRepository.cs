using SmartMenza.Data.Entities;

namespace SmartMenza.Data.Repositories.Interfaces
{
    public interface IDailyFoodIntakeRepository
    {
        Task<List<DailyFoodIntake>> GetTodayByUserIdAsync(int userId, DateTime todayUtcDate);
        Task<DailyFoodIntake> AddAsync(DailyFoodIntake entity);
        Task<DailyFoodIntake?> GetByIdAsync(int id);
        Task DeleteAsync(DailyFoodIntake entity);
    }
}
