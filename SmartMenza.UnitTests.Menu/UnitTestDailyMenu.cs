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
        
        private readonly AppDBContext _context;
        private readonly DailyMenuController _dailyMenuController;
        private readonly DailyMenuServices _dailyMenuServices;

        public UnitTestDailyMenu()
        {
            
            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDBContext(options);

            
            SeedTestData();

            _dailyMenuServices = new DailyMenuServices(_context);
            _dailyMenuController = new DailyMenuController(_dailyMenuServices);
        }

        private void SeedTestData()
        {
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
                imgPath = null
                // REMOVED: nutricionalValueId
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
                imgPath = null
                // REMOVED: nutricionalValueId
            };

            _context.Dishes.AddRange(dish1, dish2);

          
            var todayMenu = new DailyMenuDto
            {
                dailyMenuId = 1,
                date = DateOnly.FromDateTime(DateTime.Today)
                // REMOVED: dishId and dish navigation
            };

           
            var fixedDateMenu = new DailyMenuDto
            {
                dailyMenuId = 2,
                date = new DateOnly(2025, 1, 22)
                // REMOVED: dishId and dish navigation
            };

            _context.DailyMenus.AddRange(todayMenu, fixedDateMenu);
            _context.SaveChanges();

            // NOVO: Many-to-many veze kroz DailyMenuDish
            _context.DailyMenuDishes.AddRange(
                new DailyMenuDishDto { dailyMenuId = 1, dishId = 1 },
                new DailyMenuDishDto { dailyMenuId = 2, dishId = 2 }
            );

            _context.SaveChanges();
        }

        [Fact]
        public async Task GetTodaysMenuAsync_ReturnsOkAndListOfDailyMenuListItemResponse()
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