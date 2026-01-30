using Microsoft.EntityFrameworkCore;
using SmartMenza.Data.Data;
using SmartMenza.Data.Entities;
using SmartMenza.Data.Repositories.Interfaces;

namespace SmartMenza.Data.Repositories.Implementations
{
    public sealed class DishRepository : IDishRepository
    {
        private readonly AppDBContext _context;

        public DishRepository(AppDBContext context) => _context = context;

        public Task<Dish?> GetDishWithDetailsAsync(int id)
            => _context.Dishes
                .Include(d => d.DishIngredients).ThenInclude(di => di.Ingredient)
                .Include(d => d.DishRatings).ThenInclude(dr => dr.User)
                .FirstOrDefaultAsync(d => d.DishId == id);

        public Task<List<Dish>> GetAllAsync()
            => _context.Dishes.ToListAsync();

        public Task<Dish?> GetByIdAsync(int id)
            => _context.Dishes.FirstOrDefaultAsync(d => d.DishId == id);

        public Task AddAsync(Dish dish)
            => _context.Dishes.AddAsync(dish).AsTask();

        public void Remove(Dish dish)
            => _context.Dishes.Remove(dish);

        public Task SaveChangesAsync()
            => _context.SaveChangesAsync();
    }
}