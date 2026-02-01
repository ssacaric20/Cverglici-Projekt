using SmartMenza.Business.Models;
using SmartMenza.Data.Entities;
using SmartMenza.Data.Repositories.Interfaces;

namespace SmartMenza.Business.Services
{
    public class NutritionGoalService : INutritionGoalService
    {
        private readonly INutritionGoalRepository _repository;

        public NutritionGoalService(INutritionGoalRepository repository)
        {
            _repository = repository;
        }

        public async Task<NutritionGoalResponse> GetMyGoalAsync(int userId)
        {
            var latestGoal = await _repository.GetLatestByUserIdAsync(userId);

            if (latestGoal == null)
            {
                return new NutritionGoalResponse
                {
                    CaloriesGoal = 2000,
                    ProteinsGoal = 150,
                    FatsGoal = 65,
                    CarbohydratesGoal = 250,
                    GoalSetDate = DateTime.UtcNow
                };
            }

            return Map(latestGoal);
        }

        public async Task<NutritionGoalResponse> SetMyGoalAsync(int userId, SetNutritionGoalRequest request)
        {
            Validate(request);

            var entity = new NutricionGoal
            {
                UserId = userId,
                CaloriesGoal = request.CaloriesGoal,
                ProteinsGoal = request.ProteinsGoal,
                FatsGoal = request.FatsGoal,
                CarbohydratesGoal = request.CarbohydratesGoal,
                GoalSetDate = DateTime.UtcNow
            };

            var saved = await _repository.AddAsync(entity);
            return Map(saved);
        }

        private static NutritionGoalResponse Map(NutricionGoal goal)
        {
            return new NutritionGoalResponse
            {
                CaloriesGoal = goal.CaloriesGoal,
                ProteinsGoal = goal.ProteinsGoal,
                FatsGoal = goal.FatsGoal,
                CarbohydratesGoal = goal.CarbohydratesGoal,
                GoalSetDate = goal.GoalSetDate
            };
        }

        private static void Validate(SetNutritionGoalRequest request)
        {
          
            if (request.CaloriesGoal <= 0 || request.CaloriesGoal > 10000)
                throw new ArgumentException("Kalorije moraju biti u rasponu 1–10000.");

            if (request.ProteinsGoal < 0 || request.ProteinsGoal > 1000)
                throw new ArgumentException("Proteini moraju biti u rasponu 0–1000 g.");

            if (request.FatsGoal < 0 || request.FatsGoal > 1000)
                throw new ArgumentException("Masti moraju biti u rasponu 0–1000 g.");

            if (request.CarbohydratesGoal < 0 || request.CarbohydratesGoal > 1000)
                throw new ArgumentException("Ugljikohidrati moraju biti u rasponu 0–1000 g.");


            var caloriesFromMacros =
                (request.ProteinsGoal * 4) +
                (request.CarbohydratesGoal * 4) +
                (request.FatsGoal * 9);

           
            const decimal tolerance = 50;

            if (caloriesFromMacros - request.CaloriesGoal > tolerance)
            {
                throw new ArgumentException(
                    $"Zadani makronutrijenti daju {caloriesFromMacros:0} kcal, " +
                    $"što premašuje zadani cilj od {request.CaloriesGoal} kcal."
                );
            }

           
            if (request.CaloriesGoal - caloriesFromMacros > 1000m)
            {
                throw new ArgumentException(
                    "Zadani makronutrijenti daju znatno manje kalorija od cilja. " +
                    "Provjeri unos."
                );
            }
        }

    }
}
