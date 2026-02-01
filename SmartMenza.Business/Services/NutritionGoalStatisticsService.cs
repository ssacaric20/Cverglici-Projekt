using SmartMenza.Business.Models;
using SmartMenza.Business.Services.Interfaces;
using SmartMenza.Data.Repositories.Interfaces;

namespace SmartMenza.Business.Services
{
    public class NutritionGoalStatisticsService : INutritionGoalStatisticsService
    {
        private readonly IDailyFoodIntakeRepository _dailyFoodIntakeRepository;

        public NutritionGoalStatisticsService(IDailyFoodIntakeRepository dailyFoodIntakeRepository)
        {
            _dailyFoodIntakeRepository = dailyFoodIntakeRepository;
        }

        public async Task<TodayNutritionProgressResponse> GetTodayProgressAsync(int userId)
        {
            var today = DateTime.UtcNow.Date;

            var intakes = await _dailyFoodIntakeRepository.GetTodayByUserIdAsync(userId, today);

            return new TodayNutritionProgressResponse
            {
                CaloriesConsumed = intakes.Sum(x => x.Dish.Calories),
                ProteinsConsumed = intakes.Sum(x => x.Dish.Protein),
                FatsConsumed = intakes.Sum(x => x.Dish.Fat),
                CarbohydratesConsumed = intakes.Sum(x => x.Dish.Carbohydrates)
            };
        }
    }
}
