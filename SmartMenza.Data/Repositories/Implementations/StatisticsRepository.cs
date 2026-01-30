using Microsoft.EntityFrameworkCore;
using SmartMenza.Data.Data;
using SmartMenza.Data.Entities;
using SmartMenza.Data.Repositories.Interfaces;

namespace SmartMenza.Data.Repositories.Implementations
{
    public sealed class StatisticsRepository : IStatisticsRepository
    {
        private readonly AppDBContext _context;

        public StatisticsRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task<List<Dish>> GetTopRatedDishesAsync(int count)
        {
            return await _context.Dishes
                .Include(d => d.DishRatings)
                .Include(d => d.FavoriteDishes)
                .Where(d => d.DishRatings.Any())
                .OrderByDescending(d => d.DishRatings.Average(r => (double)r.Rating))
                .ThenByDescending(d => d.DishRatings.Count)
                .Take(count)
                .ToListAsync();
        }

        public async Task<List<Dish>> GetMostFavoritedDishesAsync(int count)
        {
            return await _context.Dishes
                .Include(d => d.DishRatings)
                .Include(d => d.FavoriteDishes)
                .Where(d => d.FavoriteDishes.Any())
                .OrderByDescending(d => d.FavoriteDishes.Count)
                .ThenByDescending(d => d.DishRatings.Any()
                    ? d.DishRatings.Average(r => (double)r.Rating)
                    : 0)
                .Take(count)
                .ToListAsync();
        }

        public async Task<int> GetFavoriteCountAsync(int dishId)
        {
            return await _context.FavoriteDishes
                .Where(f => f.DishId == dishId)
                .CountAsync();
        }
    }
}