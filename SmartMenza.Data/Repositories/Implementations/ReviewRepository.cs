using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartMenza.Data.Data;
using SmartMenza.Data.Entities;
using SmartMenza.Data.Repositories.Interfaces;

namespace SmartMenza.Data.Repositories.Implementations
{
    public sealed class ReviewRepository : IReviewRepository
    {
        private readonly AppDBContext _context;

        public ReviewRepository(AppDBContext context)
        {
            _context = context;
        }

        public Task<List<DishRating>> GetReviewsByDishIdAsync(int dishId) =>
            _context.DishRatings
                .Where(r => r.DishId == dishId)
                .Include(r => r.User)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

        public Task<DishRating?> GetByIdAsync(int reviewId) =>
            _context.DishRatings
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.DishRatingId == reviewId);

        public Task<DishRating?> GetUserReviewForDishAsync(int userId, int dishId) =>
            _context.DishRatings
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.UserId == userId && r.DishId == dishId);

        public Task<bool> UserHasReviewedDishAsync(int userId, int dishId) =>
            _context.DishRatings.AnyAsync(r => r.UserId == userId && r.DishId == dishId);

        public async Task AddAsync(DishRating review) =>
            await _context.DishRatings.AddAsync(review);

        public void Update(DishRating review) =>
            _context.DishRatings.Update(review);

        public void Remove(DishRating review) =>
            _context.DishRatings.Remove(review);

        public Task SaveChangesAsync() =>
            _context.SaveChangesAsync();
    }
}