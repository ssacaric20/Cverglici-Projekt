using Microsoft.AspNetCore.Mvc;
using SmartMenza.Business.Models.Favorites;
using SmartMenza.Business.Services.Interfaces;

namespace SmartMenza.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FavoriteController : ControllerBase
    {
        private readonly IFavoriteService _favoriteService;

        public FavoriteController(IFavoriteService favoriteService)
        {
            _favoriteService = favoriteService;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<List<FavoriteDishResponse>>> GetUserFavorites(int userId)
        {
            try
            {
                var favorites = await _favoriteService.GetUserFavoritesAsync(userId);
                return Ok(favorites);
            } catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error fetching favorites",
                    error = ex.Message
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddFavorite([FromBody] AddFavoriteRequest request)
        {
            try
            {
                var result = await _favoriteService.AddFavoriteAsync(request.UserId, request.DishId);

                if (!result)
                    return BadRequest(new { message = "Favorite already exists" });

                return Ok(new { message = "Favorite added successfully" });
            } catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error adding favorite",
                    error = ex.Message
                });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveFavorite([FromQuery] int userId, [FromQuery] int dishId)
        {
            try
            {
                var result = await _favoriteService.RemoveFavoriteAsync(userId, dishId);

                if (!result)
                    return NotFound(new { message = "Favorite not found" });

                return Ok(new { message = "Favorite removed successfully" });
            } catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error removing favorite",
                    error = ex.Message
                });
            }
        }

        [HttpGet("check")]
        public async Task<ActionResult<bool>> IsFavorite([FromQuery] int userId, [FromQuery] int dishId)
        {
            try
            {
                var isFavorite = await _favoriteService.IsFavoriteAsync(userId, dishId);
                return Ok(new { isFavorite });
            } catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error checking favorite status",
                    error = ex.Message
                });
            }
        }
    }

    public class AddFavoriteRequest
    {
        public int UserId { get; set; }
        public int DishId { get; set; }
    }
}