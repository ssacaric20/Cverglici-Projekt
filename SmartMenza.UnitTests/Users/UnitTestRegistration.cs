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
    public class UnitTestRegistration
    {
        private readonly AppDBContext _context;
        private readonly IUserRepository _userRepository;
        private readonly UserService _userService;
        private readonly UserController _userController;

        public UnitTestRegistration()
        {
            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDBContext(options);

            _context.Users.Add(new User
            {
                FirstName = "Existing",
                LastName = "User",
                Email = "existing@test.com",
                PasswordHash = "pass123",
                RoleId = (int)UserRole.Student
            });

            _context.SaveChanges();

            var tokenService = new FakeTokenService();

            _userRepository = new UserRepository(_context);
            _userService = new UserService(_userRepository,tokenService);
            _userController = new UserController(_userService);
        }

        [Fact]
        public async Task Register_EmptyEmail_ReturnsBadRequest()
        {
            var request = new RegistrationRequest
            {
                Name = "Test User",
                Email = "",
                Password = "test1234"
            };

            var result = await _userController.Register(request);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task Register_EmailAlreadyExists_ReturnsBadRequest()
        {
            var request = new RegistrationRequest
            {
                Name = "New User",
                Email = "existing@test.com",
                Password = "somepassword"
            };

            var result = await _userController.Register(request);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task Register_ValidInput_ReturnsOkAndPersistsUser()
        {
            var request = new RegistrationRequest
            {
                Name = "New User",
                Email = "newuser@test.com",
                Password = "test1234"
            };

            var result = await _userController.Register(request);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode);

            var response = Assert.IsType<LoginResponse>(okResult.Value);
            Assert.Equal("Registration successful", response.Message);
            Assert.NotEqual(0, response.UserId);
            Assert.False(string.IsNullOrWhiteSpace(response.Token));

            var userInDb = await _context.Users.FirstOrDefaultAsync(u => u.Email == "newuser@test.com");
            Assert.NotNull(userInDb);
            Assert.Equal("New User", userInDb!.FirstName);
            Assert.Equal((int)UserRole.Student, userInDb.RoleId);
        }
    }
}
