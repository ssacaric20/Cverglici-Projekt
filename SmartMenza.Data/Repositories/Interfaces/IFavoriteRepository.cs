using SmartMenza.Data.Entities;

namespace SmartMenza.Data.Repositories.Interfaces
{
    public interface IFavoriteRepository
    {
        Task<List<FavoriteDish>> GetByUserIdAsync(int userId);
        Task<bool> ExistsAsync(int userId, int dishId);
        Task AddAsync(FavoriteDish favorite);
        Task<FavoriteDish?> GetAsync(int userId, int dishId);
        void Remove(FavoriteDish favorite);
        Task SaveChangesAsync();
    }
}
