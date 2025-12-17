using Google.Apis.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SmartMenza.Core;
using SmartMenza.Core.Enums;
using SmartMenza.Data;
using SmartMenza.Data.Data;
using SmartMenza.Business.Models;
using SmartMenza.Data.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using SmartMenza.Business.Services.Interfaces;
using SmartMenza.Business.Models.Auth;
using SmartMenza.Business.Models.Users;



namespace SmartMenza.Business.Services
{
    public class UserServices : IUserService
    {
        private readonly AppDBContext _context;

        public UserServices(AppDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserListItemResponse>> GetUsersAsync()
        {
            var users = await _context.Users.ToListAsync();

            if (users == null || !users.Any())
            {
                // Umjesto Exceptiona vracamo praznu listu
                return Enumerable.Empty<UserListItemResponse>();
            }

            return users.Select(u => new UserListItemResponse
            {
                UserId = u.userId,
                Email = u.email,
                FirstName = u.firstName,
                LastName = u.lastName,
                RoleId = u.roleId
            }).ToList();
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
                token = token,
                roleId = user.roleId
            };

            return loginResponse;

        }

        public async Task<LoginResponse?> RegisterUserAsync(RegistrationRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.name) ||
                string.IsNullOrWhiteSpace(request.email) ||
                string.IsNullOrWhiteSpace(request.password))
            {
                return null;
            }

            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.email == request.email);

            if (existingUser != null)
            {
                return null; 
            }

            var newUser = new UserDto
            {
                email = request.email,
                firstName = request.name,   
                lastName = string.Empty,
                passwordHash = request.password, // Ovdje kasnije hashing možemo
                roleId = (int)UserRole.Student // Trenutno registriramo samo studente
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            var token = GenerateJwtToken(newUser);

            return new LoginResponse
            {
                userId = newUser.userId,
                message = "Registration successful",
                token = token,
                roleId = newUser.roleId
            };
        }

        public async Task<LoginResponse?> LoginGoogleAsync(GoogleLoginRequest request)
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings
            {
                // TODO: prebaciti pravi Google Client ID u konfiguraciju (appsettings.json)
                Audience = new[] { "STAVITI GOOGLE CLIENT ID" }
            };

            GoogleJsonWebSignature.Payload payload;

            try
            {
                payload = await GoogleJsonWebSignature.ValidateAsync(request.tokenId, settings);
            }
            catch (InvalidJwtException)
            {
                return null;
            }
            
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.email == payload.Email);

            if (user == null)
            {
                user = CreateUserForGoogleRegistration(payload);
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }

            var token = GenerateJwtToken(user);

            //vraćamo isti response kao i u mail login i registraciji
            return new LoginResponse
            {
                userId = user.userId,
                message = "Login successful",
                token = token,
                roleId = user.roleId   
            };
        }

        private UserDto CreateUserForGoogleRegistration(GoogleJsonWebSignature.Payload payload)
        {
            return new UserDto
            {
                email = payload.Email,
                firstName = payload.GivenName,
                lastName = payload.FamilyName,
                passwordHash = string.Empty,
                roleId = (int)UserRole.Student
            };
        }


        public bool IsLoginInputEmpty(LoginRequest request)
        {
            return string.IsNullOrWhiteSpace(request.email)
                || string.IsNullOrWhiteSpace(request.passwordHash);
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
