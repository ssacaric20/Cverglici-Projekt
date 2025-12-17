using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartMenza.API.Controllers;
using SmartMenza.Business.Models.Dishes;
using SmartMenza.Business.Services;
using SmartMenza.Data.Data;
using SmartMenza.Data.Models;

namespace SmartMenza.UnitTest.Dishes
{
    public class UnitTestDishDetails
    {
        private readonly AppDBContext _context;
        private readonly DishController _dishController;
        private readonly DishServices _dishServices;

        public UnitTestDishDetails()
        {
            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDBContext(options);

            SeedTestData();

            _dishServices = new DishServices(_context);
            _dishController = new DishController(_dishServices);
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
            };

            _context.Dishes.AddRange(dish1, dish2);

            _context.SaveChanges();
        }

        [Fact]
        public async Task GetDishDetails_InvalidId_Returns404()
        {
          
            int id = -1;

            
            var result = await _dishController.GetDishDetails(id);

           
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.NotNull(notFoundResult.Value);
        }

        [Fact]
        public async Task GetDishDetails_ValidId_ReturnsOkWithDishDetailsResponseDto()
        {
           
            int id = 1;

           
            var result = await _dishController.GetDishDetails(id);

           
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var dto = Assert.IsType<DishDetailsResponse>(okResult.Value);

            Assert.Equal(id, dto.DishId);
            Assert.Equal("Chicken with rice", dto.Title);
        }
    }
}
