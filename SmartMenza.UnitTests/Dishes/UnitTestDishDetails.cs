using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using SmartMenza.API.Controllers;
using SmartMenza.Business.Models.Dishes;
using SmartMenza.Business.Services;
using SmartMenza.Business.Services.Interfaces;
using SmartMenza.Data.Data;
using SmartMenza.Data.Entities;
using SmartMenza.Data.Repositories.Implementations;
using SmartMenza.Data.Repositories.Interfaces;

namespace SmartMenza.UnitTests.Dishes
{
    public class UnitTestDishDetails
    {
        private readonly AppDBContext _context;
        private readonly IDishRepository _dishRepository;
        private readonly DishService _dishService;
        private readonly DishController _dishController;

        public UnitTestDishDetails()
        {
            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDBContext(options);

            SeedTestData();

            _dishRepository = new DishRepository(_context);
            _dishService = new DishService(_dishRepository);

            var imageServiceMock = new Mock<IImageService>();
            _dishController = new DishController(_dishService, imageServiceMock.Object);
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
