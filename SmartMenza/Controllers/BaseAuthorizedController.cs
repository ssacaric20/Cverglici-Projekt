using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace SmartMenza.API.Controllers
{
    [ApiController]
    public abstract class BaseAuthorizedController : ControllerBase
    {
        protected int GetUserId()
        {
            var raw = User.FindFirstValue(ClaimTypes.NameIdentifier)
                      ?? User.FindFirstValue("userId")
                      ?? User.FindFirstValue("id");

            if (string.IsNullOrWhiteSpace(raw))
                throw new UnauthorizedAccessException("UserId claim missing.");

            return int.Parse(raw);
        }
    }
}
