using SmartMenza.Business.Models.Dishes;

namespace SmartMenza.Business.Services.Interfaces
{
    public interface IDishService
    {
        Task<DishDetailsResponse?> GetDishDetailsAsync(int id);
        Task<IEnumerable<DishListResponse>> GetAllDishesAsync();
        Task<DishDetailsResponse?> CreateDishAsync(CreateDishRequest request);
        Task<DishDetailsResponse?> UpdateDishAsync(int id, UpdateDishRequest request);
        Task<bool> DeleteDishAsync(int id);

    }
}
