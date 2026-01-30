using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartMenza.Data.Entities;

namespace SmartMenza.Data.Repositories.Interfaces
{
    public interface IReviewRepository
    {
        Task<List<DishRating>> GetReviewsByDishIdAsync(int dishId);
        Task<DishRating?> GetByIdAsync(int reviewId);
        Task<DishRating?> GetUserReviewForDishAsync(int userId, int dishId);
        Task<bool> UserHasReviewedDishAsync(int userId, int dishId);
        Task AddAsync(DishRating review);
        void Update(DishRating review);
        void Remove(DishRating review);
        Task SaveChangesAsync();
    }
}