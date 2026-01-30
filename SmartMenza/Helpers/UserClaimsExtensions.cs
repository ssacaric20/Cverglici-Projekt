using System.Security.Claims;

namespace SmartMenza.API.Helpers
{
    public static class UserClaimsExtensions
    {
        public static int GetUserId(this ClaimsPrincipal user)
        {
            var idString = user.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(idString, out var userId))
                throw new InvalidOperationException("UserId claim is missing or invalid.");

            return userId;
        }
    }
}
