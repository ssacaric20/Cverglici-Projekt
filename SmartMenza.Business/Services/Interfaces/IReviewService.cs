using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartMenza.Business.Models.Reviews;

namespace SmartMenza.Business.Services.Interfaces
{
    public interface IReviewService
    {
        Task<List<DishReviewResponse>> GetReviewsByDishIdAsync(int dishId);
        Task<DishReviewResponse?> CreateReviewAsync(int userId, int dishId, CreateReviewRequest request);
        Task<DishReviewResponse?> UpdateReviewAsync(int userId, int reviewId, UpdateReviewRequest request);
        Task<bool> DeleteReviewAsync(int userId, int reviewId);
    }
}