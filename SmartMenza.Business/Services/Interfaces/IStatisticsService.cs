using SmartMenza.Business.Models.Statistics;

namespace SmartMenza.Business.Services.Interfaces
{
    public interface IStatisticsService
    {
        Task<TopDishesResponse> GetTopDishesAsync(int count = 10);
        Task<DishStatisticsResponse?> GetDishStatisticsAsync(int dishId);
    }
}