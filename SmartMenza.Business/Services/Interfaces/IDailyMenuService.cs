using SmartMenza.Business.Models.DailyMenu;
using SmartMenza.Core.Enums;

namespace SmartMenza.Business.Services.Interfaces
{
    public interface IDailyMenuService
    {
        Task<List<DailyMenuListItemResponse>> GetTodaysMenuAsync(MenuCategory? category = null);
        Task<List<DailyMenuListItemResponse>?> GetMenuForDateAsync(string date, MenuCategory? category = null);
        Task<MenusByCategoryResponse> GetTodaysMenusGroupedAsync();

        Task<DailyMenuDetailsResponse?> GetDailyMenuByIdAsync(int id);
        Task<DailyMenuDetailsResponse?> CreateDailyMenuAsync(CreateDailyMenuRequest request);
        Task<DailyMenuDetailsResponse?> UpdateDailyMenuAsync(int id, UpdateDailyMenuRequest request);
        Task<bool> DeleteDailyMenuAsync(int id);
    }
}