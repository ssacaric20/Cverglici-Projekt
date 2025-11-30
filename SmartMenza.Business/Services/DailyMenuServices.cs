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

        /// <summary>
        /// Dohvat dnevnog menija za današnji dan.
        /// </summary>
        public async Task<IReadOnlyList<DailyMenuListItemResponse>> GetTodaysMenuAsync()
        {
            var today = DateOnly.FromDateTime(DateTime.Now);

            var dailyMenu = await _context.DailyMenus
                .Include(dm => dm.dish)
                .Where(dm => dm.date == today)
                .Select(dm => new DailyMenuListItemResponse
                {
                    DishId = dm.dishId,
                    Date = dm.date,
                    Jelo = new DailyMenuDishListItemResponse
                    {
                        DishId = dm.dish.dishId,
                        Title = dm.dish.title,
                        Price = dm.dish.price,
                        Description = dm.dish.description,
                        Calories = dm.dish.calories,
                        Protein = dm.dish.protein,
                        Fat = dm.dish.fat,
                        Carbohydrates = dm.dish.carbohydrates,
                        ImgPath = dm.dish.imgPath
                    }
                })
                .ToListAsync();

            return dailyMenu;
        }

        /// <summary>
        /// Dohvat dnevnog menija za zadani datum (yyyy-MM-dd).
        /// Ako je format datuma neispravan, vraća null.
        /// </summary>
        public async Task<IReadOnlyList<DailyMenuListItemResponse>?> GetMenuForDateAsync(string date)
        {
            if (!DateOnly.TryParse(date, out DateOnly parsedDate))
            {
                return null;
            }

            var menu = await _context.DailyMenus
                .Include(dm => dm.dish)
                .Where(dm => dm.date == parsedDate)
                .Select(dm => new DailyMenuListItemResponse
                {
                    DishId = dm.dishId,
                    Date = dm.date,
                    Jelo = new DailyMenuDishListItemResponse
                    {
                        DishId = dm.dish.dishId,
                        Title = dm.dish.title,
                        Price = dm.dish.price,
                        Description = dm.dish.description,
                        Calories = dm.dish.calories,
                        Protein = dm.dish.protein,
                        Fat = dm.dish.fat,
                        Carbohydrates = dm.dish.carbohydrates,
                        ImgPath = dm.dish.imgPath
                    }
                })
                .ToListAsync();

            return menu;
        }
    }
}
