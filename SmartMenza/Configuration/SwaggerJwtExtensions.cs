using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SmartMenza.API.Configuration;

public static class SwaggerJwtExtensions
{
    public static void AddJwtSwagger(this SwaggerGenOptions c)
    {
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Unesi JWT token ovako: Bearer {token}"
        });

        // lokotić samo na [Authorize]
        c.OperationFilter<AuthorizeOperationFilter>();
    }
}

