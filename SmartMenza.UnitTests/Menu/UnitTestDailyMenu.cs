using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartMenza.API.Controllers;
using SmartMenza.Business.Models.DailyMenu;
using SmartMenza.Business.Services;
using SmartMenza.Data.Data;
using SmartMenza.Data.Entities;
using SmartMenza.Data.Repositories.Implementations;
using SmartMenza.Data.Repositories.Interfaces;

namespace SmartMenza.UnitTests.Menu
{
    public class UnitTestDailyMenu
    {
        private readonly AppDBContext _context;
        private readonly IDailyMenuRepository _dailyMenuRepository;
        private readonly DailyMenuService _dailyMenuService;
        private readonly DailyMenuController _dailyMenuController;

        public UnitTestDailyMenu()
        {
            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDBContext(options);

            SeedTestData();

            _dailyMenuRepository = new DailyMenuRepository(_context);
            _dailyMenuService = new DailyMenuService(_dailyMenuRepository);
            _dailyMenuController = new DailyMenuController(_dailyMenuService);
        }

        private void SeedTestData()
        {
            var dish1 = new Dish
            {
                DishId = 1,
                Title = "Chicken with rice",
                Description = "Boiled chicken with white rice",
                Price = 3.20m,
                Calories = 540,
                Protein = 35m,
                Carbohydrates = 50m,
                Fat = 12m,
                Fiber = 10m,
                ImgPath = null
            };

            var dish2 = new Dish
            {
                DishId = 2,
                Title = "Vegetarian pasta",
                Description = "Pasta with vegetables",
                Price = 2.80m,
                Calories = 450,
                Protein = 15m,
                Carbohydrates = 70m,
                Fat = 8m,
                Fiber = 12m,
                ImgPath = null
            };

            _context.Dishes.AddRange(dish1, dish2);

            var today = DateOnly.FromDateTime(DateTime.Today);

            var todayLunch = new DailyMenu
            {
                DailyMenuId = 1,
                Date = today,
                Category = 1 // Lunch
            };

            var fixedDateDinner = new DailyMenu
            {
                DailyMenuId = 2,
                Date = new DateOnly(2025, 1, 22),
                Category = 2 // Dinner
            };

            _context.DailyMenus.AddRange(todayLunch, fixedDateDinner);
            _context.SaveChanges();

            _context.DailyMenuDishes.AddRange(
                new DailyMenuDish { DailyMenuId = 1, DishId = 1 },
                new DailyMenuDish { DailyMenuId = 2, DishId = 2 }
            );

            _context.SaveChanges();
        }

        [Fact]
        public async Task GetTodaysMenuAsync_ReturnsOkAndList()
        {
            var result = await _dailyMenuController.GetTodaysMenuAsync();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var value = Assert.IsAssignableFrom<IEnumerable<DailyMenuListItemResponse>>(okResult.Value);

            Assert.NotNull(value);
            Assert.NotEmpty(value);
        }

        [Fact]
        public async Task GetMenuForDateAsync_ValidDate_ReturnsOkAndList()
        {
            string date = "2025-01-22";

            var result = await _dailyMenuController.GetMenuForDateAsync(date);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var value = Assert.IsAssignableFrom<IEnumerable<DailyMenuListItemResponse>>(okResult.Value);

            Assert.NotNull(value);
            Assert.NotEmpty(value);
        }

        [Fact]
        public async Task GetMenuForDateAsync_InvalidDateInput_ReturnsBadRequest()
        {
            string date = "22-01-202b";

            var result = await _dailyMenuController.GetMenuForDateAsync(date, null);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.NotNull(badRequestResult.Value);
        }

        [Fact]
        public async Task GetMenuForDateAsync_DateWithoutMenu_ReturnsOkWithEmptyList()
        {
            string date = "2030-01-01";

            var result = await _dailyMenuController.GetMenuForDateAsync(date);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var value = Assert.IsAssignableFrom<IEnumerable<DailyMenuListItemResponse>>(okResult.Value);

            Assert.NotNull(value);
            Assert.Empty(value);
        }
    }
}
