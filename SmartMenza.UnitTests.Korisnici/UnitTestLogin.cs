using Xunit;
using System.Threading.Tasks;
using SmartMenza.Data.Data;
using SmartMenza.API.Controllers;
using Microsoft.EntityFrameworkCore;
using SmartMenza.Data.Models;
using Microsoft.AspNetCore.Mvc;
using SmartMenza.Core.Enums;
using SmartMenza.Business.Services;
using SmartMenza.Business.Models.Auth;

namespace SmartMenza.UnitTests.Korisnici
{
    public class UnitTestLogin
    {
        
        private readonly AppDBContext _context;
        private readonly UserController _userController;
        private readonly UserServices _userServices;

        public UnitTestLogin()
        {
            
            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: "SmartMenzaTestDB_Login")
                .Options;

           
            _context = new AppDBContext(options);

            
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
                    roleId = (int)UserRole.Employee
                });

            
            _context.SaveChanges();

            
            _userServices = new UserServices(_context);
            _userController = new UserController(_userServices);
        }

       
        [Fact]
        public async Task LoginUserAsync_EmptyInput_ReturnsUnauthorized()
        {
           
            var request = new LoginRequest
            {
                email = "",
                passwordHash = ""
            };

          
            var result = await _userController.LoginUserAsync(request);

          
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal(401, unauthorizedResult.StatusCode);
        }

      
        [Fact]
        public async Task LoginUserAsync_EmptyEmail_ReturnsUnauthorized()
        {
         
            var request = new LoginRequest
            {
                email = "",
                passwordHash = "test1234"
            };

        
            var result = await _userController.LoginUserAsync(request);

        
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal(401, unauthorizedResult.StatusCode);
        }

       
        [Fact]
        public async Task LoginUserAsync_EmptyPassword_ReturnsUnauthorized()
        {
          
            var request = new LoginRequest
            {
                email = "test@gmail.com",
                passwordHash = ""
            };

         
            var result = await _userController.LoginUserAsync(request);

           
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal(401, unauthorizedResult.StatusCode);
        }

       
        [Fact]
        public async Task LoginUserAsync_WrongPassword_ReturnsUnauthorized()
        {
           
            var request = new LoginRequest
            {
                email = "test@gmail.com",
                passwordHash = "test12345"
            };

          
            var result = await _userController.LoginUserAsync(request);

         
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal(401, unauthorizedResult.StatusCode);
        }

       
        [Fact]
        public async Task LoginUserAsync_CorrectInput_ReturnsOk()
        {
         
            var request = new LoginRequest
            {
                email = "test@gmail.com",
                passwordHash = "test1234"
            };

         
            var result = await _userController.LoginUserAsync(request);

        
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }

    
        [Fact]
        public async Task LoginUserAsync_CorrectEmailOtherUsersPassword_ReturnsUnauthorized()
        {
         
            var request = new LoginRequest
            {
                email = "test@gmail.com",
                passwordHash = "radnikpass"
            };

         
            var result = await _userController.LoginUserAsync(request);

          
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal(401, unauthorizedResult.StatusCode);
        }
    }
}
