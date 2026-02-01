using Moq;
using SmartMenza.Business.Models;
using SmartMenza.Business.Services;
using SmartMenza.Data.Entities;
using SmartMenza.Data.Repositories.Interfaces;

namespace SmartMenza.UnitTests.NutritionGoals
{
    public class UnitTestNutritionGoalService
    {
        private readonly NutritionGoalService _service;

        public UnitTestNutritionGoalService()
        {
            var repoMock = new Mock<INutritionGoalRepository>();

            repoMock
                .Setup(r => r.AddAsync(It.IsAny<NutricionGoal>()))
                .ReturnsAsync((NutricionGoal g) => g);

            _service = new NutritionGoalService(repoMock.Object);
        }

        [Fact]
        public async Task SetGoal_Valid_4_4_9_ShouldPass()
        {
            var request = new SetNutritionGoalRequest
            {
                CaloriesGoal = 2000,
                ProteinsGoal = 150,
                CarbohydratesGoal = 200,
                FatsGoal = 60
            };

            var ex = await Record.ExceptionAsync(() =>
                _service.SetMyGoalAsync(1, request)
            );

            Assert.Null(ex);
        }


        [Fact]
        public async Task SetGoal_MacrosExceedCalories_ShouldThrow()
        {
            var request = new SetNutritionGoalRequest
            {
                CaloriesGoal = 2000,
                ProteinsGoal = 200,
                CarbohydratesGoal = 200,
                FatsGoal = 80
            };

            var ex = await Assert.ThrowsAsync<ArgumentException>(() =>
                _service.SetMyGoalAsync(1, request)
            );

            Assert.Contains("makronutrijenti daju", ex.Message);
        }

        [Fact]
        public async Task SetGoal_NegativeValues_ShouldThrow()
        {
            var request = new SetNutritionGoalRequest
            {
                CaloriesGoal = -100,
                ProteinsGoal = 10,
                CarbohydratesGoal = 10,
                FatsGoal = 10
            };

            await Assert.ThrowsAsync<ArgumentException>(() =>
                _service.SetMyGoalAsync(1, request)
            );
        }
    }
}
