using Xunit;
using System.Threading.Tasks;
using SmartMenza.Data.Data;
using SmartMenza.API.Controllers;
using Microsoft.EntityFrameworkCore;
using SmartMenza.Data.Models;
using Microsoft.AspNetCore.Mvc;
using SmartMenza.Core.Enums;
using SmartMenza.Business.Services;
using SmartMenza.Data.Data.Entities;


namespace SmartMenza.UnitTests.Korisnici
{
    public class UnitTestLogin
    {
        // Postavljanje okoline za testiranje
        private readonly AppDBContext _context;
        private readonly UserController _userController;
        private readonly UserServices _userServices;

        public UnitTestLogin()
        {
            // Postavljanje privremene baze
            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: "SmartMenzaTestDB")
                .Options;

            // Deklaracija baze
            _context = new AppDBContext(options);

            // Dodavanje testnih korisnika u bazu
            _context.Users.Add(
                new UserDto
                {
                    firstName = "Test",
                    lastName = "Testic",
                    email = "test@gmail.com",
                    passwordHash = "test1234",
                    roleId = (int)UserRole.Student
                });
            _context.Users.Add(
                new UserDto
                {
                    firstName = "Radnik",
                    lastName = "Radnikic",
                    email = "radnik@gmail.com",
                    passwordHash = "radnikpass",
                    roleId= (int)UserRole.Employee
                });

            // Spremanje korisnika u bazu
            _context.SaveChanges();

            // Instanciranje kontrolera sa privremenom bazom kako bi se mogle testirati funkcije
            _userServices = new UserServices(_context);
            _userController = new UserController(_userServices);
        }

        // Testira funkciju LoginKorisnik sa prazim poljima u zahtjevu
        [Fact]
        public async Task LoginUserAsync_EmptyInput_ReturnsUnauthorized()
        {
            // Arrange
            var request = new LoginRequest
            {
                email = "",
                passwordHash = ""
            };

            // Act
            var result = await _userController.LoginUserAsync(request);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal(401, unauthorizedResult.StatusCode);
        }

        // Testira funkciju LoginKorisnik sa praznim email poljem u zahtjevu
        [Fact]
        public async Task LoginUserAsync_EmptyEmail_ReturnsUnauthorized()
        {
            // Arrange
            var request = new LoginRequest
            {
                email = "",
                passwordHash = "test1234"
            };

            // Act
            var result = await _userController.LoginUserAsync(request);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal(401, unauthorizedResult.StatusCode);
        }

        // Testira funkciju LoginKorisnik sa praznim lozinka poljem u zahtjevu
        [Fact]
        public async Task LoginUserAsync_EmptyPassword_ReturnsUnauthorized()
        {
            // Arrange
            var request = new LoginRequest
            {
                email = "test@gmail.com",
                passwordHash = ""
            };

            // Act
            var result = await _userController.LoginUserAsync(request);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal(401, unauthorizedResult.StatusCode);
        }

        // Testira funkciju LoginKorisnik sa pogrešnom lozinkom u zahtjevu
        [Fact]
        public async Task LoginUserAsync_WrongPassword_ReturnUnauthorized()
        {
            // Arrange
            var request = new LoginRequest
            {
                email = "test@gmail.com",
                passwordHash = "test12345"
            };

            // Act
            var result = await _userController.LoginUserAsync(request);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal(401, unauthorizedResult.StatusCode);
        }

        // Testira funkciju LoginKorisnik sa podacima postojeceg korisnika u zahtjevu
        [Fact]
        public async Task LoginUserAsync_CorrectInput_ReturnsOk()
        {
            // Arrange
            var request = new LoginRequest
            {
                email = "test@gmail.com",
                passwordHash = "test1234"
            };

            // Act
            var result = await _userController.LoginUserAsync(request);

            // Assert
            var okResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }

        // Testira funkciju LoginKorisnik sa ispravnim emailom ali lozinkom drugog korisnika u zahtjevu
        [Fact]
        public async Task LoginUserAsync_CorrectEmailOtherUsersPassword_ReturnsUnauthorized()
        {
            // Arrange
            var request = new LoginRequest
            {
                email = "test@gmail.com",
                passwordHash = "radnikpass"
            };

            // Act
            var result = await _userController.LoginUserAsync(request);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal(401, unauthorizedResult.StatusCode);
        }
    }
}
