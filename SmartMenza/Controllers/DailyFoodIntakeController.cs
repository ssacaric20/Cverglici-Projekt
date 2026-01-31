using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartMenza.Business.Models;
using SmartMenza.Business.Services.Interfaces;

namespace SmartMenza.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DailyFoodIntakeController : BaseAuthorizedController
    {
        private readonly IDailyFoodIntakeService _service;

        public DailyFoodIntakeController(IDailyFoodIntakeService service)
        {
            _service = service;
        }

        [HttpGet("today")]
        public async Task<IActionResult> GetMyToday()
        {
            var userId = GetUserId();
            return Ok(await _service.GetMyTodayAsync(userId));
        }

        [HttpPost("today")]
        public async Task<IActionResult> AddToMyToday([FromBody] AddDailyFoodIntakeRequest request)
        {
            try
            {
                var userId = GetUserId();
                return Ok(await _service.AddToMyTodayAsync(userId, request));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{dailyFoodIntakeId:int}")]
        public async Task<IActionResult> Delete(int dailyFoodIntakeId)
        {
            try
            {
                var userId = GetUserId();
                await _service.RemoveFromMyTodayAsync(userId, dailyFoodIntakeId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new { message = ex.Message }); 
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }




    }
}
