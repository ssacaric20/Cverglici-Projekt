using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartMenza.Business.Models.Statistics;
using SmartMenza.Business.Services.Interfaces;

namespace SmartMenza.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public sealed class StatisticsController : ControllerBase
    {
        private readonly IStatisticsService _statisticsService;

        public StatisticsController(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }

        [HttpGet("top-dishes")]
        public async Task<ActionResult<TopDishesResponse>> GetTopDishes([FromQuery] int count = 10)
        {
            try
            {
                if (count <= 0 || count > 50)
                {
                    return BadRequest(new { message = "Count must be between 1 and 50" });
                }

                var statistics = await _statisticsService.GetTopDishesAsync(count);
                return Ok(statistics);
            } catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error fetching statistics",
                    error = ex.Message
                });
            }
        }

        [HttpGet("dish/{dishId}")]
        public async Task<ActionResult<DishStatisticsResponse>> GetDishStatistics(int dishId)
        {
            try
            {
                var statistics = await _statisticsService.GetDishStatisticsAsync(dishId);

                if (statistics == null)
                {
                    return NotFound(new { message = "Dish not found or has no statistics" });
                }

                return Ok(statistics);
            } catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error fetching dish statistics",
                    error = ex.Message
                });
            }
        }
    }
}