using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartMenza.API.Controllers;
using SmartMenza.Business.Models.Auth;
using SmartMenza.Business.Models.Users;
using SmartMenza.Business.Services;
using SmartMenza.Business.Services.Interfaces;
using SmartMenza.Core.Enums;
using SmartMenza.Data.Data;
using SmartMenza.Data.Models;
using Xunit;

namespace SmartMenza.UnitTests.Korisnici
{
    public class UnitTestRegistration
    {
        private readonly AppDBContext _context;
        private readonly IUserService _userServices;
        private readonly UserController _userController;

        public UnitTestRegistration()
        {
            var options = new DbContextOptionsBuilder<AppDBContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

            _context = new AppDBContext(options);

            
            _context.Users.Add(
                new UserDto
                {
                    firstName = "Existing",
                    lastName = "User",
                    email = "existing@test.com",
                    passwordHash = "pass123",
                    roleId = (int)UserRole.Student
                });

            _context.SaveChanges();

            _userServices = new UserServices(_context);
            _userController = new UserController(_userServices);
        }

       
        [Fact]
        public async Task Register_EmptyEmail_ReturnsBadRequest()
        {
        
            var request = new RegistrationRequest
            {
                name = "Test User",
                email = "",              // prazno -> invalid
                password = "test1234"
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
                name = "New User",
                email = "existing@test.com", 
                password = "somepassword"
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
                name = "New User",
                email = "newuser@test.com",
                password = "test1234"
            };

            
            var result = await _userController.Register(request);

            
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode);

            var response = Assert.IsType<LoginResponse>(okResult.Value);
            Assert.Equal("Registration successful", response.message);
            Assert.NotEqual(0, response.userId);
            Assert.False(string.IsNullOrWhiteSpace(response.token));

            
            var userInDb = await _context.Users.FirstOrDefaultAsync(u => u.email == "newuser@test.com");
            Assert.NotNull(userInDb);
            Assert.Equal("New User", userInDb!.firstName);  // jer u servisu: firstName = request.name
            Assert.Equal((int)UserRole.Student, userInDb.roleId);
        }
    }
}
