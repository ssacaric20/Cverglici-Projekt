using SmartMenza.Business.Models.DailyMenu;
using SmartMenza.Core.Enums;

namespace SmartMenza.Business.Services.Interfaces
{
    public interface IDailyMenuService
    {
        Task<IReadOnlyList<DailyMenuListItemResponse>> GetTodaysMenuAsync(MenuCategory? category = null);
        Task<IReadOnlyList<DailyMenuListItemResponse>?> GetMenuForDateAsync(string date, MenuCategory? category = null);
        Task<MenusByCategoryResponse> GetTodaysMenusGroupedAsync();
    }
}

