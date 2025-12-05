using Microsoft.EntityFrameworkCore;
using SmartMenza.Data.Data;
using SmartMenza.Business.Models.DailyMenu;
using SmartMenza.Core.Enums;

namespace SmartMenza.Business.Services
{
    public class DailyMenuServices
    {
        private readonly AppDBContext _context;

        public DailyMenuServices(AppDBContext context)
        {
            _context = context;
        }

        // danasnji menu za odredjenu kategoriju
        public async Task<IReadOnlyList<DailyMenuListItemResponse>> GetTodaysMenuAsync(MenuCategory? category = null)
        {
            var today = DateOnly.FromDateTime(DateTime.Now);

            var query = _context.DailyMenus
                .Include(dm => dm.dailyMenuDishes)
                    .ThenInclude(dmd => dmd.dish)
                .Where(dm => dm.date == today);

            // filter prema kategoriji
            if (category.HasValue)
            {
                query = query.Where(dm => dm.category == (int)category.Value);
            }

            var dailyMenu = await query
                .SelectMany(dm => dm.dailyMenuDishes.Select(dmd => new DailyMenuListItemResponse
                {
                    DishId = dmd.dishId,
                    Date = dm.date,
                    Category = dm.category == 1 ? "Lunch" : "Dinner",
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
                        Fiber = dmd.dish.fiber,
                        ImgPath = dmd.dish.imgPath
                    }
                }))
                .ToListAsync();

            return dailyMenu;
        }
 
        // menu za specifican dan, optional kategorija
        public async Task<IReadOnlyList<DailyMenuListItemResponse>?> GetMenuForDateAsync(string date, MenuCategory? category = null)
        {
            if (!DateOnly.TryParse(date, out DateOnly parsedDate))
            {
                return null;
            }

            var query = _context.DailyMenus
                .Include(dm => dm.dailyMenuDishes)
                    .ThenInclude(dmd => dmd.dish)
                .Where(dm => dm.date == parsedDate);

            // filter by category if provided
            if (category.HasValue)
            {
                query = query.Where(dm => dm.category == (int)category.Value);
            }

            var menu = await query
                .SelectMany(dm => dm.dailyMenuDishes.Select(dmd => new DailyMenuListItemResponse
                {
                    DishId = dmd.dishId,
                    Date = dm.date,
                    Category = dm.category == 1 ? "Lunch" : "Dinner",
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
                        Fiber = dmd.dish.fiber,
                        ImgPath = dmd.dish.imgPath
                    }
                }))
                .ToListAsync();

            return menu;
        }

        // get menu za rucak i veceru
        public async Task<MenusByCategory> GetTodaysMenusGroupedAsync()
        {
            var today = DateOnly.FromDateTime(DateTime.Now);

            var allMenus = await _context.DailyMenus
                .Include(dm => dm.dailyMenuDishes)
                    .ThenInclude(dmd => dmd.dish)
                .Where(dm => dm.date == today)
                .SelectMany(dm => dm.dailyMenuDishes.Select(dmd => new
                {
                    Category = dm.category,
                    Item = new DailyMenuListItemResponse
                    {
                        DishId = dmd.dishId,
                        Date = dm.date,
                        Category = dm.category == 1 ? "Lunch" : "Dinner",
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
                    }
                }))
                .ToListAsync();

            return new MenusByCategory
            {
                Lunch = allMenus.Where(x => x.Category == 1).Select(x => x.Item).ToList(),
                Dinner = allMenus.Where(x => x.Category == 2).Select(x => x.Item).ToList()
            };
        }
    }

    // novi response model 
    public class MenusByCategory
    {
        public List<DailyMenuListItemResponse> Lunch { get; set; } = new();
        public List<DailyMenuListItemResponse> Dinner { get; set; } = new();
    }
}