using SmartMenza.Business.Models.Favorites;
using SmartMenza.Business.Services.Interfaces;
using SmartMenza.Data.Entities;
using SmartMenza.Data.Repositories.Interfaces;

namespace SmartMenza.Business.Services
{
    public sealed class FavoriteService : IFavoriteService
    {
        private readonly IFavoriteRepository _favorites;

        public FavoriteService(IFavoriteRepository favorites)
        {
            _favorites = favorites;
        }

        public async Task<List<FavoriteDishResponse>> GetUserFavoritesAsync(int userId)
        {
            var favorites = await _favorites.GetByUserIdAsync(userId);

            if (favorites == null || favorites.Count == 0)
                return new List<FavoriteDishResponse>();

            return favorites.Select(f => new FavoriteDishResponse
            {
                DishId = f.Dish.DishId,
                Title = f.Dish.Title,
                Description = f.Dish.Description,
                Price = f.Dish.Price,
                Calories = f.Dish.Calories,
                ImgPath = f.Dish.ImgPath
            }).ToList();
        }

        public async Task<bool> AddFavoriteAsync(int userId, int dishId)
        {
            if (await _favorites.ExistsAsync(userId, dishId))
                return false;

            await _favorites.AddAsync(new FavoriteDish
            {
                UserId = userId,
                DishId = dishId
            });

            await _favorites.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveFavoriteAsync(int userId, int dishId)
        {
            var fav = await _favorites.GetAsync(userId, dishId);
            if (fav == null) return false;

            _favorites.Remove(fav);
            await _favorites.SaveChangesAsync();
            return true;
        }

        public Task<bool> IsFavoriteAsync(int userId, int dishId)
            => _favorites.ExistsAsync(userId, dishId);
    }
}
