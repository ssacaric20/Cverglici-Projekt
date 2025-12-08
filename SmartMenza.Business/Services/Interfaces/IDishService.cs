using SmartMenza.Business.Models.Dishes;

namespace SmartMenza.Business.Services.Interfaces
{
    public interface IDishService
    {
        Task<DishDetailsResponse?> GetDishDetailsAsync(int id);
    }
}
