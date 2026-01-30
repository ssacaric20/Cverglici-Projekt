using SmartMenza.Business.Models.Favorites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMenza.Business.Services.Interfaces
{
    public interface IFavoriteService
    {
        Task<List<FavoriteDishResponse>> GetUserFavoritesAsync(int userId);
        Task<bool> AddFavoriteAsync(int userId, int dishId);
        Task<bool> RemoveFavoriteAsync(int userId, int dishId);
        Task<bool> IsFavoriteAsync(int userId, int dishId);
    }
}
