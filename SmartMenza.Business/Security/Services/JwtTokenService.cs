using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SmartMenza.Business.Security.Services.Interfaces;
using SmartMenza.Core.Settings;
using SmartMenza.Data.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SmartMenza.Business.Security.Services
{
    public class JwtTokenService : ITokenService
    {
        private readonly JwtSettings _settings;

        public JwtTokenService(IOptions<JwtSettings> options)
        {
            _settings = options.Value;
        }

        public string GenerateToken(User user)
        {
            if (string.IsNullOrWhiteSpace(_settings.SecretKey))
                throw new InvalidOperationException("JwtSettings:SecretKey is missing.");

            var key = Encoding.UTF8.GetBytes(_settings.SecretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.RoleId.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(_settings.ExpirationDays),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var handler = new JwtSecurityTokenHandler();
            return handler.WriteToken(handler.CreateToken(tokenDescriptor));
        }
    }
}
