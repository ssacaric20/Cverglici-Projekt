using Microsoft.EntityFrameworkCore;
using SmartMenza.Data.Data;
using SmartMenza.Data.Entities;
using SmartMenza.Data.Repositories.Interfaces;

namespace SmartMenza.Data.Repositories.Implementations
{
    public class DailyFoodIntakeRepository : IDailyFoodIntakeRepository
    {
        private readonly AppDBContext _db;

        public DailyFoodIntakeRepository(AppDBContext db) => _db = db;

        public Task<List<DailyFoodIntake>> GetTodayByUserIdAsync(int userId, DateTime todayUtcDate)
        {
            var start = todayUtcDate.Date;
            var end = start.AddDays(1);

            return _db.DailyFoodIntakes
                .Include(x => x.Dish)
                .Where(x => x.UserId == userId && x.Date >= start && x.Date < end)
                .OrderByDescending(x => x.DailyFoodIntakeId)
                .ToListAsync();
        }


        public async Task<DailyFoodIntake> AddAsync(DailyFoodIntake entity)
        {
            _db.DailyFoodIntakes.Add(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public Task<DailyFoodIntake?> GetByIdAsync(int id) =>
            _db.DailyFoodIntakes
               .Include(x => x.Dish)
               .FirstOrDefaultAsync(x => x.DailyFoodIntakeId == id);

        public async Task DeleteAsync(DailyFoodIntake entity)
        {
            _db.DailyFoodIntakes.Remove(entity);
            await _db.SaveChangesAsync();
        }
    }
}
