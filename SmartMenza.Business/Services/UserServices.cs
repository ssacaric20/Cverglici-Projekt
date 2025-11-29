using Google.Apis.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SmartMenza.Core;
using SmartMenza.Core.Enums;
using SmartMenza.Data;
using SmartMenza.Data.Data;
using SmartMenza.Data.Data.Entities;
using SmartMenza.Data.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace SmartMenza.Business.Services
{
    public class UserServices
    {
        private readonly AppDBContext _context;

        public UserServices(AppDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserDto>> GetUsersAsync()
        {
            var users = await _context.Users.ToListAsync();

            if (users == null || !users.Any())
            {
                throw new Exception("No users found.");
            }

            return users;
        }

        public async Task<LoginResponse?> LoginUserAsync(LoginRequest request)
        {
            bool formIsEmpty = IsLoginInputEmpty(request);

            if (formIsEmpty) return null;

            var user = await _context.Users.FirstOrDefaultAsync(u => u.email == request.email && u.passwordHash == request.passwordHash);

            if (user == null) return null;

            var token = GenerateJwtToken(user);

            var loginResponse = new LoginResponse
            {
                userId = user.userId,
                message = "Login successful",
                token = token
            };

            return loginResponse;

        }

        public async Task<UserDto?> RegisterUserAsync(RegisterRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.email) ||
                string.IsNullOrWhiteSpace(request.password) ||
                string.IsNullOrWhiteSpace(request.firstName) ||
                string.IsNullOrWhiteSpace(request.lastName))
            {
                return null;
            }

            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.email == request.email);

            if (existingUser != null)
            {
                return null;
            }

            string hashedPassword = HashPassword(request.password);

            var newUser = new UserDto
            {
                email = request.email,
                passwordHash = hashedPassword,
                firstName = request.firstName,
                lastName = request.lastName,
                roleId = request.roleId
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return newUser;
        }

        private string HashPassword(string password)
        {
            return password;
        }


        public async Task<UserDto?> LoginGoogleAsync(GoogleLoginRequest request)
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                // GOOGLE CLIENT ID FALI!!!!!!!!!!!!
                Audience = new[] { "STAVITI GOOGLE CLIENT ID" }
            };

            var payload = await GoogleJsonWebSignature.ValidateAsync(request.tokenId, settings);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.email == payload.Email);

            if (user == null)
            {

                user = CreateUserForGoogleRegistration(payload);

                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }

            return user;
        }

        private UserDto CreateUserForGoogleRegistration(GoogleJsonWebSignature.Payload payload)
        {
            var user = new UserDto
            {
                email = payload.Email,
                firstName = payload.GivenName,
                lastName = payload.FamilyName,
                passwordHash = "",
                roleId = (int)UserRole.Student
            };

            return user;
        }

        public bool IsLoginInputEmpty(LoginRequest request)
        {
            if (request.passwordHash == "" || request.email == "")
            {
                return true;
            }
            return false;
        }

        private string GenerateJwtToken(UserDto user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("f17ca2eec6a314d8f6d80fddb6bd4135a2a5a5a715700e463f788465a221f099");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.NameIdentifier, user.userId.ToString()),
            new Claim(ClaimTypes.Email, user.email),
            new Claim(ClaimTypes.Role, user.roleId.ToString())
        }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
