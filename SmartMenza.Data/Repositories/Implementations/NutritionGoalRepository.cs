using Microsoft.EntityFrameworkCore;
using SmartMenza.Data.Data;
using SmartMenza.Data.Entities;
using SmartMenza.Data.Repositories.Interfaces;

namespace SmartMenza.Data.Repositories.Implementations
{
    public class NutritionGoalRepository : INutritionGoalRepository
    {
        private readonly AppDBContext _db;
        public NutritionGoalRepository(AppDBContext db) => _db = db;

        public Task<NutricionGoal?> GetLatestByUserIdAsync(int userId) =>
            _db.NutricionGoals
               .Where(g => g.UserId == userId)
               .OrderByDescending(g => g.GoalSetDate)
               .FirstOrDefaultAsync();

        public async Task<NutricionGoal> AddAsync(NutricionGoal goal)
        {
            _db.NutricionGoals.Add(goal);
            await _db.SaveChangesAsync();
            return goal;
        }
    }
}
