using Microsoft.EntityFrameworkCore;
using SmartMenza.Data.Data;
using SmartMenza.Data.Entities;
using SmartMenza.Data.Repositories.Interfaces;

namespace SmartMenza.Data.Repositories.Implementations
{
    public sealed class FavoriteRepository : IFavoriteRepository
    {
        private readonly AppDBContext _context;

        public FavoriteRepository(AppDBContext context)
        {
            _context = context;
        }

        public Task<List<FavoriteDish>> GetByUserIdAsync(int userId) =>
            _context.FavoriteDishes
                .Where(f => f.UserId == userId)
                .Include(f => f.Dish)
                .ToListAsync();

        public Task<bool> ExistsAsync(int userId, int dishId) =>
            _context.FavoriteDishes.AnyAsync(f => f.UserId == userId && f.DishId == dishId);

        public async Task AddAsync(FavoriteDish favorite) =>
            await _context.FavoriteDishes.AddAsync(favorite);

        public Task<FavoriteDish?> GetAsync(int userId, int dishId) =>
            _context.FavoriteDishes.FirstOrDefaultAsync(f => f.UserId == userId && f.DishId == dishId);

        public void Remove(FavoriteDish favorite) =>
            _context.FavoriteDishes.Remove(favorite);

        public Task SaveChangesAsync() =>
            _context.SaveChangesAsync();
    }
}
