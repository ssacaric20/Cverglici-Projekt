using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SmartMenza.API.Configuration
{
    public class AuthorizeOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // ako je endpoint ili controller označen s [Authorize]
            var hasAuthorize =
                context.MethodInfo.DeclaringType?.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any() == true
                || context.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any();

            // ako je endpoint označen s [AllowAnonymous], ne stavljaj requirement
            var hasAllowAnonymous =
                context.MethodInfo.GetCustomAttributes(true).OfType<AllowAnonymousAttribute>().Any();

            if (!hasAuthorize || hasAllowAnonymous)
                return;

            operation.Security ??= new List<OpenApiSecurityRequirement>();

            operation.Security.Add(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        }
    }
}
