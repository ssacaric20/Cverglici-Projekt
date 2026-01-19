using SmartMenza.Business.Models.DailyMenu;
using SmartMenza.Business.Services.Interfaces;
using SmartMenza.Core.Enums;
using SmartMenza.Data.Entities;
using SmartMenza.Data.Repositories.Interfaces;

namespace SmartMenza.Business.Services
{
    public sealed class DailyMenuService : IDailyMenuService
    {
        private readonly IDailyMenuRepository _menus;

        public DailyMenuService(IDailyMenuRepository menus) => _menus = menus;

        public async Task<List<DailyMenuListItemResponse>> GetTodaysMenuAsync(MenuCategory? category = null)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);

            var entities = category.HasValue
                ? await _menus.GetMenusForDateAndCategoryAsync(today, (int)category.Value)
                : await _menus.GetMenusForDateAsync(today);

            return MapToListItems(entities);
        }

        public async Task<List<DailyMenuListItemResponse>?> GetMenuForDateAsync(string date, MenuCategory? category = null)
        {
            if (!DateOnly.TryParse(date, out var parsed))
                return null;

            var entities = category.HasValue
                ? await _menus.GetMenusForDateAndCategoryAsync(parsed, (int)category.Value)
                : await _menus.GetMenusForDateAsync(parsed);

            return MapToListItems(entities);
        }

        public async Task<MenusByCategoryResponse> GetTodaysMenusGroupedAsync()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var menus = await _menus.GetMenusForDateAsync(today);
            var items = MapToListItems(menus);

            return new MenusByCategoryResponse
            {
                Lunch = items.Where(x => x.Category == "Lunch").ToList(),
                Dinner = items.Where(x => x.Category == "Dinner").ToList()
            };
        }

        public async Task<DailyMenuDetailsResponse?> GetDailyMenuByIdAsync(int id)
        {
            var menu = await _menus.GetByIdWithDishesAsync(id);
            return menu is null ? null : MapToDetails(menu);
        }

        public async Task<DailyMenuDetailsResponse?> CreateDailyMenuAsync(CreateDailyMenuRequest request)
        {
            if (!DateOnly.TryParse(request.Date, out var parsedDate))
                return null;

            if (await _menus.ExistsForDateAndCategoryAsync(parsedDate, request.Category))
                return null;

            var menu = new DailyMenu
            {
                Date = parsedDate,
                Category = request.Category
            };

            await _menus.AddAsync(menu);
            await _menus.SaveChangesAsync(); // da dobijemo ID

            foreach (var dishId in request.DishIds.Distinct())
                await _menus.AddMenuDishAsync(new DailyMenuDish { DailyMenuId = menu.DailyMenuId, DishId = dishId });

            await _menus.SaveChangesAsync();

            var created = await _menus.GetByIdWithDishesAsync(menu.DailyMenuId);
            return created is null ? null : MapToDetails(created);
        }

        public async Task<DailyMenuDetailsResponse?> UpdateDailyMenuAsync(int id, UpdateDailyMenuRequest request)
        {
            var menu = await _menus.GetByIdWithDishesAsync(id);
            if (menu is null) return null;

            if (!DateOnly.TryParse(request.Date, out var parsedDate))
                return null;

           
            var exists = await _menus.ExistsForDateAndCategoryAsync(parsedDate, request.Category);
            if (exists && (menu.Date != parsedDate || menu.Category != request.Category))
                return null;

            menu.Date = parsedDate;
            menu.Category = request.Category;

           
            _menus.RemoveMenuDishes(menu.DailyMenuDishes);
            foreach (var dishId in request.DishIds.Distinct())
                await _menus.AddMenuDishAsync(new DailyMenuDish { DailyMenuId = menu.DailyMenuId, DishId = dishId });

            await _menus.SaveChangesAsync();

            var updated = await _menus.GetByIdWithDishesAsync(id);
            return updated is null ? null : MapToDetails(updated);
        }

        public async Task<bool> DeleteDailyMenuAsync(int id)
        {
            var menu = await _menus.GetByIdWithDishesAsync(id);
            if (menu is null) return false;

            _menus.RemoveMenuDishes(menu.DailyMenuDishes);
            _menus.Remove(menu);

            await _menus.SaveChangesAsync();
            return true;
        }

        private static List<DailyMenuListItemResponse> MapToListItems(IEnumerable<DailyMenu> menus)
            => menus
                .SelectMany(dm => dm.DailyMenuDishes.Select(dmd => new DailyMenuListItemResponse
                {
                    DailyMenuId = dm.DailyMenuId,
                    DishId = dmd.DishId,
                    Date = dm.Date,
                    Category = dm.Category == (int)MenuCategory.Lunch ? "Lunch" : "Dinner",
                    Jelo = new DailyMenuDishListItemResponse
                    {
                        DishId = dmd.Dish.DishId,
                        Title = dmd.Dish.Title,
                        Price = dmd.Dish.Price,
                        Description = dmd.Dish.Description,
                        Calories = dmd.Dish.Calories,
                        Protein = dmd.Dish.Protein,
                        Fat = dmd.Dish.Fat,
                        Carbohydrates = dmd.Dish.Carbohydrates,
                        Fiber = dmd.Dish.Fiber,
                        ImgPath = dmd.Dish.ImgPath
                    }
                }))
                .ToList();

        private static DailyMenuDetailsResponse MapToDetails(DailyMenu menu)
            => new DailyMenuDetailsResponse
            {
                DailyMenuId = menu.DailyMenuId,
                Date = menu.Date,
                Category = menu.Category == (int)MenuCategory.Lunch ? "Lunch" : "Dinner",
                Dishes = menu.DailyMenuDishes.Select(dmd => new DailyMenuDishListItemResponse
                {
                    DishId = dmd.Dish.DishId,
                    Title = dmd.Dish.Title,
                    Price = dmd.Dish.Price,
                    Description = dmd.Dish.Description,
                    Calories = dmd.Dish.Calories,
                    Protein = dmd.Dish.Protein,
                    Fat = dmd.Dish.Fat,
                    Carbohydrates = dmd.Dish.Carbohydrates,
                    Fiber = dmd.Dish.Fiber,
                    ImgPath = dmd.Dish.ImgPath
                }).ToList()
            };
    }
}
