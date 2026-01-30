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
    public class UnitTestDishCategory
    {
        private readonly AppDBContext _context;
        private readonly IDailyMenuRepository _dailyMenuRepository;
        private readonly DailyMenuService _dailyMenuService;
        private readonly DailyMenuController _dailyMenuController;

        public UnitTestDishCategory()
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
            var dishLunch = new Dish
            {
                DishId = 10,
                Title = "Lunch Dish",
                Description = "Test lunch",
                Price = 3.00m,
                Calories = 400,
                Protein = 20m,
                Carbohydrates = 50m,
                Fat = 10m,
                Fiber = 5m,
                ImgPath = null
            };

            var dishDinner = new Dish
            {
                DishId = 20,
                Title = "Dinner Dish",
                Description = "Test dinner",
                Price = 4.00m,
                Calories = 600,
                Protein = 30m,
                Carbohydrates = 40m,
                Fat = 20m,
                Fiber = 6m,
                ImgPath = null
            };

            _context.Dishes.AddRange(dishLunch, dishDinner);

            var today = DateOnly.FromDateTime(DateTime.Today);

            var lunchMenu = new DailyMenu
            {
                DailyMenuId = 100,
                Date = today,
                Category = 1 // Lunch
            };

            var dinnerMenu = new DailyMenu
            {
                DailyMenuId = 200,
                Date = today,
                Category = 2 // Dinner
            };

            _context.DailyMenus.AddRange(lunchMenu, dinnerMenu);
            _context.SaveChanges();

            _context.DailyMenuDishes.AddRange(
                new DailyMenuDish { DailyMenuId = 100, DishId = 10 },
                new DailyMenuDish { DailyMenuId = 200, DishId = 20 }
            );

            _context.SaveChanges();
        }

        [Fact]
        public async Task GetTodaysMenuAsync_WithLunchCategory_ReturnsOnlyLunch()
        {
            // category = 1 => Lunch (ako koristiš enum u controlleru, ovdje samo proslijedi taj enum)
            var result = await _dailyMenuController.GetTodaysMenuAsync("Lunch");

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var value = Assert.IsAssignableFrom<IEnumerable<DailyMenuListItemResponse>>(okResult.Value);

            Assert.NotNull(value);
            Assert.NotEmpty(value);

            Assert.All(value, item => Assert.Equal("Lunch", item.Category));
        }

        [Fact]
        public async Task GetTodaysMenuAsync_WithDinnerCategory_ReturnsOnlyDinner()
        {
            var result = await _dailyMenuController.GetTodaysMenuAsync("Dinner");

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var value = Assert.IsAssignableFrom<IEnumerable<DailyMenuListItemResponse>>(okResult.Value);

            Assert.NotNull(value);
            Assert.NotEmpty(value);

            Assert.All(value, item => Assert.Equal("Dinner", item.Category));
        }
    }
}
