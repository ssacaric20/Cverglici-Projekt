using SmartMenza.Data.Entities;

namespace SmartMenza.Data.Repositories.Interfaces
{
    public interface IDailyMenuRepository
    {
        Task<List<DailyMenu>> GetMenusForDateAsync(DateOnly date);
        Task<List<DailyMenu>> GetMenusForDateAndCategoryAsync(DateOnly date, int category);

        Task<DailyMenu?> GetByIdWithDishesAsync(int id);
        Task<bool> ExistsForDateAndCategoryAsync(DateOnly date, int category);

        Task AddAsync(DailyMenu menu);
        void Remove(DailyMenu menu);

        // join tablica
        Task AddMenuDishAsync(DailyMenuDish menuDish);
        void RemoveMenuDishes(IEnumerable<DailyMenuDish> menuDishes);

        Task SaveChangesAsync();
    }
}
