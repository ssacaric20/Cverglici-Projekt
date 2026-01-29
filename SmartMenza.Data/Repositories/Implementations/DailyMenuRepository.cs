using Microsoft.EntityFrameworkCore;
using SmartMenza.Data.Data;
using SmartMenza.Data.Entities;
using SmartMenza.Data.Repositories.Interfaces;

namespace SmartMenza.Data.Repositories.Implementations
{
    public sealed class DailyMenuRepository : IDailyMenuRepository
    {
        private readonly AppDBContext _context;

        public DailyMenuRepository(AppDBContext context)
        {
            _context = context;
        }

        public Task<List<DailyMenu>> GetMenusForDateAsync(DateOnly date)
            => _context.DailyMenus
                .Include(dm => dm.DailyMenuDishes)
                    .ThenInclude(dmd => dmd.Dish)
                .Where(dm => dm.Date == date)
                .ToListAsync();

        public Task<List<DailyMenu>> GetMenusForDateAndCategoryAsync(DateOnly date, int category)
            => _context.DailyMenus
                .Include(dm => dm.DailyMenuDishes)
                    .ThenInclude(dmd => dmd.Dish)
                .Where(dm => dm.Date == date && dm.Category == category)
                .ToListAsync();

        public Task<DailyMenu?> GetByIdWithDishesAsync(int id)
            => _context.DailyMenus
                .Include(dm => dm.DailyMenuDishes)
                    .ThenInclude(dmd => dmd.Dish)
                .FirstOrDefaultAsync(dm => dm.DailyMenuId == id);

        public Task<bool> ExistsForDateAndCategoryAsync(DateOnly date, int category)
            => _context.DailyMenus.AnyAsync(dm => dm.Date == date && dm.Category == category);

        public Task AddAsync(DailyMenu menu)
            => _context.DailyMenus.AddAsync(menu).AsTask();

        public void Remove(DailyMenu menu)
            => _context.DailyMenus.Remove(menu);

        public Task SaveChangesAsync()
            => _context.SaveChangesAsync();

        public Task AddMenuDishAsync(DailyMenuDish menuDish)
            => _context.DailyMenuDishes.AddAsync(menuDish).AsTask();

        public void RemoveMenuDishes(IEnumerable<DailyMenuDish> menuDishes)
            => _context.DailyMenuDishes.RemoveRange(menuDishes);

    }
}
