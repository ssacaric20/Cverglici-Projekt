using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SmartMenza.Core.Settings;
using System.Text;

namespace SmartMenza.API.Configuration
{
    public static class JwtExtensions
    {
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(); 

            // Konfiguracija JwtBearerOptions preko DI (čita JwtSettings iz appsettings)
            services.AddOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme)
                .Configure<IOptions<JwtSettings>>((options, jwtOptions) =>
                {
                    var jwtSettings = jwtOptions.Value;

                    if (string.IsNullOrWhiteSpace(jwtSettings.SecretKey))
                        throw new InvalidOperationException("JwtSettings:SecretKey is missing.");

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),

                        ValidateIssuer = false,
                        ValidateAudience = false,

                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });

            return services;
        }
    }
}
