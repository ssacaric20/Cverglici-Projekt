using SmartMenza.Data.Entities;

namespace SmartMenza.Data.Repositories.Interfaces
{
    public interface IDishRepository
    {
        Task<Dish?> GetDishWithDetailsAsync(int id);
        Task<List<Dish>> GetAllAsync();
        Task<Dish?> GetByIdAsync(int id);

        Task AddAsync(Dish dish);
        void Remove(Dish dish);
        Task SaveChangesAsync();
    }
}
