using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using SmartMenza.API.Controllers;
using SmartMenza.Business.Services;
using SmartMenza.Data.Data;
using SmartMenza.Data.Models;
using System.Threading.Tasks;

namespace SmartMenza.UnitTest.Dishes
{
    public class UnitTestDishDetails
    {
        // Postavljanje okoline za testiranje
        private readonly AppDBContext _context;
        private readonly DishController _dishController;
        private readonly DishServices _dishServices;

        public UnitTestDishDetails()
        {
            // Postavljanje privremene baze
            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: "SmartMenzaTestDB")
                .Options;

            // Deklaracija baze
            _context = new AppDBContext(options);

            // Dodavanje testnih korisnika u bazu
            _context.Dishes.Add(
                new DishDto
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
                }
            );

            _context.Dishes.Add(
                new DishDto
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
                }
            );


            // Spremanje jela u bazu
            _context.SaveChanges();

            // Instanciranje kontrolera sa privremenom bazom kako bi se mogle testirati funkcije
            _dishServices = new DishServices(_context);
            _dishController = new DishController(_dishServices);
        }

        [Fact]
        public async Task GetDishDetails_InvalidId_Returns500()
        {
            // Arragne
            int id = -1;

            // Act
            var result = await _dishController.GetDishDetails(id);

            // Assert
            var badRequestResult = Assert.IsType<Microsoft.AspNetCore.Mvc.NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetDishDetails_ValidId_Returns200()
        {
            // Arrange
            int id = 1;

            // Act
            var result = await _dishController.GetDishDetails(id);

            // Assert
            var okRequestResult = Assert.IsType<Microsoft.AspNetCore.Mvc.OkObjectResult>(result.Result);
        }
    }
}