using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartMenza.API.Helpers;
using SmartMenza.Business.Models.Favorites;
using SmartMenza.Business.Services.Interfaces;

namespace SmartMenza.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public sealed class FavoriteController : ControllerBase
    {
        private readonly IFavoriteService _favoriteService;

        public FavoriteController(IFavoriteService favoriteService)
        {
            _favoriteService = favoriteService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<FavoriteDishResponse>>> GetMyFavorites()
        {
            var userId = User.GetUserId();
            return Ok(await _favoriteService.GetUserFavoritesAsync(userId));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddFavorite([FromBody] AddFavoriteRequest request)
        {
            var userId = User.GetUserId();

            var ok = await _favoriteService.AddFavoriteAsync(userId, request.DishId);
            if (!ok) return BadRequest(new { message = "Favorite already exists" });

            return Ok(new { message = "Favorite added successfully" });
        }

        [HttpDelete("{dishId:int}")]
        public async Task<IActionResult> RemoveFavorite(int dishId)
        {
            var userId = User.GetUserId();

            var ok = await _favoriteService.RemoveFavoriteAsync(userId, dishId);
            if (!ok) return NotFound(new { message = "Favorite not found" });

            return Ok(new { message = "Favorite removed successfully" });
        }

        [HttpGet("check/{dishId:int}")]
        public async Task<IActionResult> IsFavorite(int dishId)
        {
            var userId = User.GetUserId();
            return Ok(new { isFavorite = await _favoriteService.IsFavoriteAsync(userId, dishId) });
        }
    }
}
