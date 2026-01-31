using SmartMenza.Business.Models;
using SmartMenza.Business.Services.Interfaces;
using SmartMenza.Data.Entities;
using SmartMenza.Data.Repositories.Interfaces;

namespace SmartMenza.Business.Services
{
    public class DailyFoodIntakeService : IDailyFoodIntakeService
    {
        private readonly IDailyFoodIntakeRepository _repo;

        public DailyFoodIntakeService(IDailyFoodIntakeRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<DailyFoodIntakeResponse>> GetMyTodayAsync(int userId)
        {
            var today = DateTime.UtcNow.Date;
            var items = await _repo.GetTodayByUserIdAsync(userId, today);
            return items.Select(Map).ToList();
        }

        public async Task<DailyFoodIntakeResponse> AddToMyTodayAsync(int userId, AddDailyFoodIntakeRequest request)
        {
            if (request.DishId <= 0)
                throw new ArgumentException("Neispravan DishId.");

            var entity = new DailyFoodIntake
            {
                UserId = userId,
                DishId = request.DishId,
                Date = DateTime.UtcNow
            };

            var saved = await _repo.AddAsync(entity);

            // da dobijemo Dish podatke (Include) – najjednostavnije re-fetch
            var withDish = await _repo.GetByIdAsync(saved.DailyFoodIntakeId);
            if (withDish == null) throw new Exception("Spremanje nije uspjelo.");

            return Map(withDish);
        }

        public async Task RemoveFromMyTodayAsync(int userId, int dailyFoodIntakeId)
        {
            var entity = await _repo.GetByIdAsync(dailyFoodIntakeId);
            if (entity == null) throw new KeyNotFoundException("Stavka nije pronađena.");
            if (entity.UserId != userId) throw new UnauthorizedAccessException("Nemaš pravo brisanja.");

            await _repo.DeleteAsync(entity);
        }

        private static DailyFoodIntakeResponse Map(DailyFoodIntake x) => new()
        {
            DailyFoodIntakeId = x.DailyFoodIntakeId,
            Date = x.Date,
            DishId = x.DishId,
            DishTitle = x.Dish.Title,
            Calories = x.Dish.Calories,
            Protein = x.Dish.Protein,
            Fat = x.Dish.Fat,
            Carbohydrates = x.Dish.Carbohydrates,
            ImgPath = x.Dish.ImgPath
        };
    }
}
