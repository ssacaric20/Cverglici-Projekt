using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartMenza.API.Controllers;
using SmartMenza.Business.Models.DailyMenu;
using SmartMenza.Business.Services;
using SmartMenza.Data.Data;
using SmartMenza.Data.Models;

namespace SmartMenza.UnitTests.Menu
{
    public class UnitTestDailyMenu
    {
        // Postavljanje okoline za testiranje
        private readonly AppDBContext _context;
        private readonly DailyMenuController _dailyMenuController;
        private readonly DailyMenuServices _dailyMenuServices;

        public UnitTestDailyMenu()
        {
            // Svaki testni context dobije svoju in-memory bazu
            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDBContext(options);

            // Seed testnih podataka
            SeedTestData();

            _dailyMenuServices = new DailyMenuServices(_context);
            _dailyMenuController = new DailyMenuController(_dailyMenuServices);
        }

        private void SeedTestData()
        {
            // Jedno jelo
            var dish1 = new DishDto
            {
                dishId = 1,
                title = "Chicken with rice",
                description = "Boiled chicken with white rice",
                price = 3.20m,
                calories = 540,
                protein = 35m,
                carbohydrates = 50m,
                fat = 12m,
                imgPath = null,
                nutricionalValueId = 1
            };

            var dish2 = new DishDto
            {
                dishId = 2,
                title = "Vegetarian pasta",
                description = "Pasta with vegetables",
                price = 2.80m,
                calories = 450,
                protein = 15m,
                carbohydrates = 70m,
                fat = 8m,
                imgPath = null,
                nutricionalValueId = 1
            };

            _context.Dishes.AddRange(dish1, dish2);

            // Dnevni meni za današnji dan
            _context.DailyMenus.Add(new DailyMenuDto
            {
                dailyMenuId = 1,
                date = DateOnly.FromDateTime(DateTime.Today),
                dishId = dish1.dishId,
                dish = dish1
            });

            // Dnevni meni za fiksni datum (2025-01-22)
            _context.DailyMenus.Add(new DailyMenuDto
            {
                dailyMenuId = 2,
                date = new DateOnly(2025, 1, 22),
                dishId = dish2.dishId,
                dish = dish2
            });

            _context.SaveChanges();
        }

        [Fact]
        public async Task GetTodaysMenuAsync_ReturnsOkAndListOfDailyMenuListItemResponse()
        {
            // Act
            var result = await _dailyMenuController.GetTodaysMenuAsync();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var value = Assert.IsAssignableFrom<IEnumerable<DailyMenuListItemResponse>>(okResult.Value);

            Assert.NotNull(value);
            Assert.NotEmpty(value);  // imamo seed za današnji dan
        }

        [Fact]
        public async Task GetMenuForDateAsync_ValidDate_ReturnsOkAndList()
        {
            // Arrange
            string date = "2025-01-22";

            // Act
            var result = await _dailyMenuController.GetMenuForDateAsync(date);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var value = Assert.IsAssignableFrom<IEnumerable<DailyMenuListItemResponse>>(okResult.Value);

            Assert.NotNull(value);
            Assert.NotEmpty(value); // imamo seed za taj datum
        }

        [Fact]
        public async Task GetMenuForDateAsync_WrongDateFormat_ReturnsBadRequest()
        {
            // Arrange
            string date = "22-01-2025"; // krivi format

            // Act
            var result = await _dailyMenuController.GetMenuForDateAsync(date);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.NotNull(badRequestResult.Value);
        }

        [Fact]
        public async Task GetMenuForDateAsync_DateWithoutMenu_ReturnsOkWithEmptyList()
        {
            // Arrange – datum za koji nismo seedali meni
            string date = "2030-01-01";

            // Act
            var result = await _dailyMenuController.GetMenuForDateAsync(date);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var value = Assert.IsAssignableFrom<IEnumerable<DailyMenuListItemResponse>>(okResult.Value);

            Assert.NotNull(value);
            Assert.Empty(value); // nema menija za taj datum
        }
    }
}
