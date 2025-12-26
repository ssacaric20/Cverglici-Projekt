using Microsoft.EntityFrameworkCore;
using SmartMenza.Business.Models.Dishes;
using SmartMenza.Business.Services.Interfaces;
using SmartMenza.Data.Data;
using SmartMenza.Data.Models;


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

        public async Task<IEnumerable<DishListResponse>> GetAllDishesAsync()
        {
            var dishes = await _context.Dishes
                .Select(d => new DishListResponse
                {
                    DishId = d.dishId,
                    Title = d.title,
                    Price = d.price,
                    Description = d.description,
                    Calories = d.calories,
                    ImgPath = d.imgPath
                })
                .ToListAsync();

            return dishes;
        }

        public async Task<DishDetailsResponse?> CreateDishAsync(CreateDishRequest request)
        {
            var newDish = new DishDto
            {
                title = request.Title,
                price = request.Price,
                description = request.Description,
                calories = request.Calories,
                protein = request.Protein,
                fat = request.Fat,
                carbohydrates = request.Carbohydrates,
                fiber = request.Fiber,
                imgPath = request.ImgPath
            };

            _context.Dishes.Add(newDish);
            await _context.SaveChangesAsync();

            return await GetDishDetailsAsync(newDish.dishId);
        }

        public async Task<DishDetailsResponse?> UpdateDishAsync(int id, UpdateDishRequest request)
        {
            var dish = await _context.Dishes.FindAsync(id);

            if (dish == null)
            {
                return null;
            }

            dish.title = request.Title;
            dish.price = request.Price;
            dish.description = request.Description;
            dish.calories = request.Calories;
            dish.protein = request.Protein;
            dish.fat = request.Fat;
            dish.carbohydrates = request.Carbohydrates;
            dish.fiber = request.Fiber;
            dish.imgPath = request.ImgPath;

            _context.Dishes.Update(dish);
            await _context.SaveChangesAsync();

            return await GetDishDetailsAsync(dish.dishId);
        }

        public async Task<bool> DeleteDishAsync(int id)
        {
            var dish = await _context.Dishes.FindAsync(id);

            if (dish == null)
            {
                return false;
            }

            _context.Dishes.Remove(dish);
            await _context.SaveChangesAsync();

            return true;
        }

    }
}
