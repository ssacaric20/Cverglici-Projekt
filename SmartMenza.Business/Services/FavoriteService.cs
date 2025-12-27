using Microsoft.EntityFrameworkCore;
using SmartMenza.Business.Models.Favorites;
using SmartMenza.Business.Services.Interfaces;
using SmartMenza.Data.Data;
using SmartMenza.Data.Models;

namespace SmartMenza.Business.Services
{
    public class FavoriteService : IFavoriteService
    {
        private readonly AppDBContext _context;

        public FavoriteService(AppDBContext context)
        {
            _context = context;
        }

        public async Task<List<FavoriteDishResponse>> GetUserFavoritesAsync(int userId)
        {
            var favorites = await _context.FavoriteDishes
                .Where(f => f.userId == userId)
                .Include(f => f.dish)
                .Select(f => new FavoriteDishResponse
                {
                    DishId = f.dish.dishId,
                    Title = f.dish.title,
                    Description = f.dish.description,
                    Price = f.dish.price,
                    Calories = f.dish.calories,
                    ImgPath = f.dish.imgPath
                })
                .ToListAsync();

            return favorites;
        }

        public async Task<bool> AddFavoriteAsync(int userId, int dishId)
        {
            var existingFavorite = await _context.FavoriteDishes
                .FirstOrDefaultAsync(f => f.userId == userId && f.dishId == dishId);

            if (existingFavorite != null)
                return false;

            var favorite = new FavoriteDishDto
            {
                userId = userId,
                dishId = dishId
            };

            _context.FavoriteDishes.Add(favorite);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveFavoriteAsync(int userId, int dishId)
        {
            var favorite = await _context.FavoriteDishes
                .FirstOrDefaultAsync(f => f.userId == userId && f.dishId == dishId);

            if (favorite == null)
                return false;

            _context.FavoriteDishes.Remove(favorite);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IsFavoriteAsync(int userId, int dishId)
        {
            return await _context.FavoriteDishes
                .AnyAsync(f => f.userId == userId && f.dishId == dishId);
        }
    }
}
