using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartMenza.Business.Models;
using SmartMenza.Business.Services;

namespace SmartMenza.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NutritionGoalController : BaseAuthorizedController
    {
        private readonly INutritionGoalService _service;

        public NutritionGoalController(INutritionGoalService service)
        {
            _service = service;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMyGoal()
        {
            var userId = GetUserId();
            var response = await _service.GetMyGoalAsync(userId);
            return Ok(response);
        }

        [HttpPut("me")]
        public async Task<IActionResult> SetMyGoal([FromBody] SetNutritionGoalRequest request)
        {
            try
            {
                var userId = GetUserId();
                var response = await _service.SetMyGoalAsync(userId, request);
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        
    }
}
