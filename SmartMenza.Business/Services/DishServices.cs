using Microsoft.EntityFrameworkCore;
using SmartMenza.Data.Data;
using SmartMenza.Business.Models.Dishes;
using SmartMenza.Business.Services.Interfaces;


namespace SmartMenza.Business.Services
{
    public class DishServices : IDishService
    {
        private readonly AppDBContext _context;

        public DishServices(AppDBContext context)
        {
            _context = context;
        }

        public async Task<DishDetailsResponse?> GetDishDetailsAsync(int id)
        {
            var dish = await _context.Dishes
                .Include(d => d.dishIngredients)
                .ThenInclude(di => di.ingredient)
                .Include(d => d.dishRatings)
                .FirstOrDefaultAsync(d => d.dishId == id);

            if (dish == null)
                return null;

          
            var ingredientNames = dish.dishIngredients
                .Select(di => di.ingredient.name)
                .Distinct()
                .ToList();

            
            int ratingsCount = dish.dishRatings?.Count ?? 0;
            double? averageRating = null;

            if (ratingsCount > 0)
            {
                averageRating = dish.dishRatings.Average(r => (double)r.rating);
            }

            return new DishDetailsResponse
            {
                DishId = dish.dishId,
                Title = dish.title,
                Description = dish.description,
                Price = dish.price,
                Calories = dish.calories,
                Protein = dish.protein,
                Fat = dish.fat,
                Carbohydrates = dish.carbohydrates,
                Fiber = dish.fiber,
                ImgPath = dish.imgPath,

                Ingredients = ingredientNames,
                AverageRating = averageRating,
                RatingsCount = ratingsCount
            };
        }
    }
}
