using Microsoft.EntityFrameworkCore;
using SmartMenza.Data.Data;
using SmartMenza.Business.Models.DailyMenu;

namespace SmartMenza.Business.Services
{
    public class DailyMenuServices
    {
        private readonly AppDBContext _context;

        public DailyMenuServices(AppDBContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<DailyMenuListItemResponse>> GetTodaysMenuAsync()
        {
            var today = DateOnly.FromDateTime(DateTime.Now);

            var dailyMenu = await _context.DailyMenus
                .Include(dm => dm.dailyMenuDishes)
                    .ThenInclude(dmd => dmd.dish)
                .Where(dm => dm.date == today)
                .SelectMany(dm => dm.dailyMenuDishes.Select(dmd => new DailyMenuListItemResponse
                {
                    DishId = dmd.dishId,
                    Date = dm.date,
                    Jelo = new DailyMenuDishListItemResponse
                    {
                        DishId = dmd.dish.dishId,
                        Title = dmd.dish.title,
                        Price = dmd.dish.price,
                        Description = dmd.dish.description,
                        Calories = dmd.dish.calories,
                        Protein = dmd.dish.protein,
                        Fat = dmd.dish.fat,
                        Carbohydrates = dmd.dish.carbohydrates,
                        ImgPath = dmd.dish.imgPath
                    }
                }))
                .ToListAsync();

            return dailyMenu;
        }

        public async Task<IReadOnlyList<DailyMenuListItemResponse>?> GetMenuForDateAsync(string date)
        {
            if (!DateOnly.TryParse(date, out DateOnly parsedDate))
            {
                return null;
            }

            var menu = await _context.DailyMenus
                .Include(dm => dm.dailyMenuDishes)
                    .ThenInclude(dmd => dmd.dish)
                .Where(dm => dm.date == parsedDate)
                .SelectMany(dm => dm.dailyMenuDishes.Select(dmd => new DailyMenuListItemResponse
                {
                    DishId = dmd.dishId,
                    Date = dm.date,
                    Jelo = new DailyMenuDishListItemResponse
                    {
                        DishId = dmd.dish.dishId,
                        Title = dmd.dish.title,
                        Price = dmd.dish.price,
                        Description = dmd.dish.description,
                        Calories = dmd.dish.calories,
                        Protein = dmd.dish.protein,
                        Fat = dmd.dish.fat,
                        Carbohydrates = dmd.dish.carbohydrates,
                        ImgPath = dmd.dish.imgPath
                    }
                }))
                .ToListAsync();

            return menu;
        }
    }
}
