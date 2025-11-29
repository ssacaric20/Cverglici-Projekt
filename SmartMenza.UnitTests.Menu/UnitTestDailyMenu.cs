using SmartMenza.Data.Data;
using SmartMenza.API.Controllers;
using SmartMenza.Data.Models;
using SmartMenza.Business.Services;
using SmartMenza.Core.Enums;
using Microsoft.EntityFrameworkCore;

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
            // Postavljanje privremene baze
            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: "SmartMenzaTestDB")
                .Options;

            // Deklaracija baze
            _context = new AppDBContext(options);

            // Dodavanje testnih korisnika u bazu
            //_context.DailyMenus.Add(
            //    new DailyMenuDto
            //    {
            //        dailyMenuId = 1,
            //        date = DateOnly.FromDateTime(DateTime.Now),
            //        dishId = 1,
            //        dish = new DishDto
            //        {
            //            dishId = 1,
            //            title = "Chicken with rice",
            //            description = "Boiled chicken with white rice",
            //            price = 3.20m,
            //            calories = 540,
            //            protein = 35m,
            //            carbohydrates = 50m,
            //            fat = 12m,
            //            imgPath = null,
            //            nutricionalValueId = 1
            //        }
            //    }

            //    );
            //_context.DailyMenus.Add(
            //    new DailyMenuDto
            //    {
            //        dailyMenuId = 2,
            //        date = new DateOnly(2025, 1, 22),
            //        dishId = 1,
            //        dish = new DishDto
            //        {
            //            dishId = 2,
            //            title = "Vegetarian pasta",
            //            description = "Pasta with vegetables",
            //            price = 2.80m,
            //            calories = 450,
            //            protein = 15m,
            //            carbohydrates = 70m,
            //            fat = 8m,
            //            imgPath = null,
            //            nutricionalValueId = 1
            //        }
            //    });

            // Spremanje korisnika u bazu
            _context.SaveChanges();

            // Instanciranje kontrolera sa privremenom bazom kako bi se mogle testirati funkcije
            _dailyMenuServices = new DailyMenuServices(_context);
            _dailyMenuController = new DailyMenuController(_dailyMenuServices);
        }

        [Fact]
        public async Task GetTodaysMenuAsync_ReturnsTodaysMenu()
        {
            // Arrange

            // Act
            var result = await _dailyMenuController.GetTodaysMenuAsync();

            // Assert
            var okResult = Assert.IsType<Microsoft.AspNetCore.Mvc.OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetMenuForDateAsync_DateIs2025_1_22_ReturnsMenuForGivenDate()
        {
            // Arrange
            string date = "2025-01-22";

            // Act
            var result = await _dailyMenuController.GetMenuForDateAsync(date);

            // Assert
            var okResult = Assert.IsType<Microsoft.AspNetCore.Mvc.OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetMenuForDateAsync_WrongDateFormat_ReturnsBadRequest()
        {
            // Arrange
            string date = "22-01-2025";

            // Act
            var result = await _dailyMenuController.GetMenuForDateAsync(date);

            // Assert
            var badRequestResult = Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetMenuForDateAsync_DateOutOfRange_ReturnsBadRequest()
        {
            // Arrange
            string date = "1111-1111-1111";

            // Act
            var result = await _dailyMenuController.GetMenuForDateAsync(date);

            // Assert
            var badRequestResult = Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(result.Result);
        }
    }
}