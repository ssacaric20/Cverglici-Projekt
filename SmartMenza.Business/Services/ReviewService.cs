using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartMenza.Business.Models.Reviews;
using SmartMenza.Business.Services.Interfaces;
using SmartMenza.Data.Entities;
using SmartMenza.Data.Repositories.Interfaces;

namespace SmartMenza.Business.Services
{
    public sealed class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviews;
        private readonly IDishRepository _dishes;

        public ReviewService(IReviewRepository reviews, IDishRepository dishes)
        {
            _reviews = reviews;
            _dishes = dishes;
        }

        public async Task<List<DishReviewResponse>> GetReviewsByDishIdAsync(int dishId)
        {
            var reviews = await _reviews.GetReviewsByDishIdAsync(dishId);

            return reviews.Select(r => new DishReviewResponse
            {
                DishRatingId = r.DishRatingId,
                Rating = r.Rating,
                Comment = r.Comment,
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt,
                UserId = r.UserId,
                UserName = $"{r.User.FirstName} {r.User.LastName}"
            }).ToList();
        }

        public async Task<DishReviewResponse?> CreateReviewAsync(int userId, int dishId, CreateReviewRequest request)
        {
            // Provjeri postoji li jelo
            var dish = await _dishes.GetByIdAsync(dishId);
            if (dish == null) return null;

            // Provjeri je li korisnik već ostavio recenziju
            var existingReview = await _reviews.GetUserReviewForDishAsync(userId, dishId);
            if (existingReview != null) return null;

            var review = new DishRating
            {
                UserId = userId,
                DishId = dishId,
                Rating = request.Rating,
                Comment = request.Comment,
                CreatedAt = DateTime.UtcNow
            };

            await _reviews.AddAsync(review);
            await _reviews.SaveChangesAsync();

            // Dohvati nazad s User podacima
            var savedReview = await _reviews.GetByIdAsync(review.DishRatingId);
            if (savedReview == null) return null;

            return new DishReviewResponse
            {
                DishRatingId = savedReview.DishRatingId,
                Rating = savedReview.Rating,
                Comment = savedReview.Comment,
                CreatedAt = savedReview.CreatedAt,
                UpdatedAt = savedReview.UpdatedAt,
                UserId = savedReview.UserId,
                UserName = $"{savedReview.User.FirstName} {savedReview.User.LastName}"
            };
        }

        public async Task<DishReviewResponse?> UpdateReviewAsync(int userId, int reviewId, UpdateReviewRequest request)
        {
            var review = await _reviews.GetByIdAsync(reviewId);
            if (review == null) return null;

            // Provjeri je li korisnik vlasnik recenzije
            if (review.UserId != userId) return null;

            review.Rating = request.Rating;
            review.Comment = request.Comment;
            review.UpdatedAt = DateTime.UtcNow;

            _reviews.Update(review);
            await _reviews.SaveChangesAsync();

            return new DishReviewResponse
            {
                DishRatingId = review.DishRatingId,
                Rating = review.Rating,
                Comment = review.Comment,
                CreatedAt = review.CreatedAt,
                UpdatedAt = review.UpdatedAt,
                UserId = review.UserId,
                UserName = $"{review.User.FirstName} {review.User.LastName}"
            };
        }

        public async Task<bool> DeleteReviewAsync(int userId, int reviewId)
        {
            var review = await _reviews.GetByIdAsync(reviewId);
            if (review == null) return false;

            // Provjeri je li korisnik vlasnik recenzije
            if (review.UserId != userId) return false;

            _reviews.Remove(review);
            await _reviews.SaveChangesAsync();
            return true;
        }
    }
}