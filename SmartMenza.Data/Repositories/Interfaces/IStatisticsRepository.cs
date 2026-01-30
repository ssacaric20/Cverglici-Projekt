using SmartMenza.Data.Entities;

namespace SmartMenza.Data.Repositories.Interfaces
{
    public interface IStatisticsRepository
    {
        Task<List<Dish>> GetTopRatedDishesAsync(int count);
        Task<List<Dish>> GetMostFavoritedDishesAsync(int count);
        Task<int> GetFavoriteCountAsync(int dishId);
    }
}
