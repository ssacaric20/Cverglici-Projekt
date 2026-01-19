using Google.Apis.Auth;
using Microsoft.AspNetCore.Authorization;
using SmartMenza.Business.Models.Auth;
using SmartMenza.Business.Models.Users;
using SmartMenza.Business.Security.Services.Interfaces;
using SmartMenza.Business.Services.Interfaces;
using SmartMenza.Core.Enums;
using SmartMenza.Data.Entities;
using SmartMenza.Data.Repositories.Interfaces;

namespace SmartMenza.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _users;

        private readonly ITokenService _tokenService;

        public UserService(IUserRepository users, ITokenService  tokenService)
        {
            _users = users;

            _tokenService = tokenService;
        }

       
        public async Task<IEnumerable<UserListItemResponse>> GetUsersAsync()
        {
            var users = await _users.GetAllAsync();

            if (users == null || users.Count == 0)
                return Enumerable.Empty<UserListItemResponse>();

            return users.Select(u => new UserListItemResponse
            {
                UserId = u.UserId,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                RoleId = u.RoleId
            }).ToList();
        }

        public async Task<LoginResponse?> LoginUserAsync(LoginRequest request)
        {
            if (IsLoginInputEmpty(request)) return null;

            // request.Password je plain, samo se zove "passwordHash" na FrontEnd radi kompatibilnosti
            var user = await _users.GetByEmailAndPasswordAsync(request.Email, request.Password);
            if (user == null) return null;

            var token = _tokenService.GenerateToken(user);

            return new LoginResponse
            {
                UserId = user.UserId,
                Message = "Login successful",
                Token = token,
                RoleId = user.RoleId
            };
        }

        public async Task<LoginResponse?> RegisterUserAsync(RegistrationRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name) ||
                string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Password))
                return null;

            var existing = await _users.GetByEmailAsync(request.Email);
            if (existing != null) return null;

            var newUser = new User
            {
                Email = request.Email,
                FirstName = request.Name,
                LastName = string.Empty,
                PasswordHash = request.Password, // trenutno plain (kasnije hashing)
                RoleId = (int)UserRole.Student
            };

            await _users.AddAsync(newUser);
            await _users.SaveChangesAsync();

            var token = _tokenService.GenerateToken(newUser); 

            return new LoginResponse
            {
                UserId = newUser.UserId,
                Message = "Registration successful",
                Token = token,
                RoleId = newUser.RoleId
            };
        }

        public async Task<LoginResponse?> LoginGoogleAsync(GoogleLoginRequest request)
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new[] { "STAVITI GOOGLE CLIENT ID" }
            };

            GoogleJsonWebSignature.Payload payload;
            try
            {
                payload = await GoogleJsonWebSignature.ValidateAsync(request.TokenId, settings);
            }
            catch (InvalidJwtException)
            {
                return null;
            }

            var user = await _users.GetByEmailAsync(payload.Email);

            if (user == null)
            {
                user = new User
                {
                    Email = payload.Email,
                    FirstName = payload.GivenName,
                    LastName = payload.FamilyName,
                    PasswordHash = string.Empty,
                    RoleId = (int)UserRole.Student
                };

                await _users.AddAsync(user);
                await _users.SaveChangesAsync();
            }

            var token = _tokenService.GenerateToken(user);

            return new LoginResponse
            {
                UserId = user.UserId,
                Message = "Login successful",
                Token = token,
                RoleId = user.RoleId
            };
        }

        public bool IsLoginInputEmpty(LoginRequest request)
            => string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password);

        
    }
}
