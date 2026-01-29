using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartMenza.API.Controllers;
using SmartMenza.Business.Models.Auth;
using SmartMenza.Business.Services;
using SmartMenza.Core.Enums;
using SmartMenza.Data.Data;
using SmartMenza.Data.Entities;
using SmartMenza.Data.Repositories.Implementations;
using SmartMenza.Data.Repositories.Interfaces;
using SmartMenza.UnitTests.Helpers;

namespace SmartMenza.UnitTests.Users
{
    public class UnitTestLogin
    {
        private readonly AppDBContext _context;
        private readonly IUserRepository _userRepository;
        private readonly UserService _userService;
        private readonly UserController _userController;

        public UnitTestLogin()
        {
            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDBContext(options);

            _context.Users.AddRange(
                new User
                {
                    FirstName = "Test",
                    LastName = "Testic",
                    Email = "test@gmail.com",
                    PasswordHash = "test1234",
                    RoleId = (int)UserRole.Student
                },
                new User
                {
                    FirstName = "Radnik",
                    LastName = "Radnikic",
                    Email = "radnik@gmail.com",
                    PasswordHash = "radnikpass",
                    RoleId = (int)UserRole.Employee
                }
            );

            _context.SaveChanges();

            var tokenService = new FakeTokenService();

            _userRepository = new UserRepository(_context);
            _userService = new UserService(_userRepository, tokenService);
            _userController = new UserController(_userService);
        }

        [Fact]
        public async Task LoginUserAsync_EmptyInput_ReturnsUnauthorized()
        {
            var request = new LoginRequest { Email = "", Password = "" };

            var result = await _userController.LoginUserAsync(request);

            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal(401, unauthorizedResult.StatusCode);
        }

        [Fact]
        public async Task LoginUserAsync_EmptyEmail_ReturnsUnauthorized()
        {
            var request = new LoginRequest { Email = "", Password = "test1234" };

            var result = await _userController.LoginUserAsync(request);

            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal(401, unauthorizedResult.StatusCode);
        }

        [Fact]
        public async Task LoginUserAsync_EmptyPassword_ReturnsUnauthorized()
        {
            var request = new LoginRequest { Email = "test@gmail.com", Password = "" };

            var result = await _userController.LoginUserAsync(request);

            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal(401, unauthorizedResult.StatusCode);
        }

        [Fact]
        public async Task LoginUserAsync_WrongPassword_ReturnsUnauthorized()
        {
            var request = new LoginRequest { Email = "test@gmail.com", Password = "test12345" };

            var result = await _userController.LoginUserAsync(request);

            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal(401, unauthorizedResult.StatusCode);
        }

        [Fact]
        public async Task LoginUserAsync_CorrectInput_ReturnsOk()
        {
            var request = new LoginRequest { Email = "test@gmail.com", Password = "test1234" };

            var result = await _userController.LoginUserAsync(request);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task LoginUserAsync_CorrectEmailOtherUsersPassword_ReturnsUnauthorized()
        {
            var request = new LoginRequest { Email = "test@gmail.com", Password = "radnikpass" };

            var result = await _userController.LoginUserAsync(request);

            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal(401, unauthorizedResult.StatusCode);
        }
    }
}
