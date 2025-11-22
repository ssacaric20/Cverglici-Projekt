using Google.Apis.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SmartMenza.Core;
using SmartMenza.Core.Enums;
using SmartMenza.Data;
using SmartMenza.Data.Data;
using SmartMenza.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<UserDto?> LoginUserAsync(LoginRequest request)
        {
            bool formIsEmpty = IsLoginInputEmpty(request);

            if (formIsEmpty) return null;

            var user = await _context.Users.FirstOrDefaultAsync(u => u.email == request.email && u.passwordHash == request.passwordHash);

            return user;

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
    }
}
