using SmartMenza.Business.Models.Dishes;
using SmartMenza.Business.Models.Reviews;
using SmartMenza.Business.Services.Interfaces;
using SmartMenza.Data.Entities;
using SmartMenza.Data.Repositories.Interfaces;

namespace SmartMenza.Business.Services
{
    public sealed class DishService : IDishService
    {
        private readonly IDishRepository _dishes;

        public DishService(IDishRepository dishes) => _dishes = dishes;

        public async Task<DishDetailsResponse?> GetDishDetailsAsync(int id)
        {
            var dish = await _dishes.GetDishWithDetailsAsync(id);
            if (dish is null) return null;

            var ingredientNames = dish.DishIngredients
                .Where(di => di.Ingredient != null)
                .Select(di => di.Ingredient.Name)
                .Distinct()
                .ToList();

            var ratingsCount = dish.DishRatings?.Count ?? 0;
            double? avg = ratingsCount > 0
                ? dish.DishRatings!.Average(r => (double)r.Rating)
                : null;

            var reviews = dish.DishRatings?
                .OrderByDescending(r => r.CreatedAt)
                .Select(r => new DishReviewResponse
                {
                    DishRatingId = r.DishRatingId,
                    Rating = r.Rating,
                    Comment = r.Comment,
                    CreatedAt = r.CreatedAt,
                    UpdatedAt = r.UpdatedAt,
                    UserId = r.UserId,
                    UserName = $"{r.User.FirstName} {r.User.LastName}"
                })
                .ToList() ?? new List<DishReviewResponse>();

            return new DishDetailsResponse
            {
                DishId = dish.DishId,
                Title = dish.Title,
                Description = dish.Description ?? string.Empty,
                Price = dish.Price,
                Calories = dish.Calories,
                Protein = dish.Protein,
                Fat = dish.Fat,
                Carbohydrates = dish.Carbohydrates,
                Fiber = dish.Fiber,
                ImgPath = dish.ImgPath,
                Ingredients = ingredientNames,
                AverageRating = avg,
                RatingsCount = ratingsCount,
                Reviews = reviews
            };
        }

        public async Task<IEnumerable<DishListResponse>> GetAllDishesAsync()
        {
            var dishes = await _dishes.GetAllAsync();
            return dishes.Select(d => new DishListResponse
            {
                DishId = d.DishId,
                Title = d.Title,
                Price = d.Price,
                Description = d.Description,
                Calories = d.Calories,
                ImgPath = d.ImgPath
            });
        }

        public async Task<DishDetailsResponse?> CreateDishAsync(CreateDishRequest request)
        {
            var dish = new Dish
            {
                Title = request.Title,
                Price = request.Price,
                Description = request.Description,
                Calories = request.Calories,
                Protein = request.Protein,
                Fat = request.Fat,
                Carbohydrates = request.Carbohydrates,
                Fiber = request.Fiber,
                ImgPath = request.ImgPath
            };

            await _dishes.AddAsync(dish);
            await _dishes.SaveChangesAsync();

            return await GetDishDetailsAsync(dish.DishId);
        }

        public async Task<DishDetailsResponse?> UpdateDishAsync(int id, UpdateDishRequest request)
        {
            var dish = await _dishes.GetByIdAsync(id);
            if (dish is null) return null;

            dish.Title = request.Title;
            dish.Price = request.Price;
            dish.Description = request.Description;
            dish.Calories = request.Calories;
            dish.Protein = request.Protein;
            dish.Fat = request.Fat;
            dish.Carbohydrates = request.Carbohydrates;
            dish.Fiber = request.Fiber;
            dish.ImgPath = request.ImgPath;

            await _dishes.SaveChangesAsync();

            return await GetDishDetailsAsync(id);
        }

        public async Task<bool> DeleteDishAsync(int id)
        {
            var dish = await _dishes.GetByIdAsync(id);
            if (dish is null) return false;

            _dishes.Remove(dish);
            await _dishes.SaveChangesAsync();
            return true;
        }
    }
}