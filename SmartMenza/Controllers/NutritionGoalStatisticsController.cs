using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartMenza.Business.Services.Interfaces;

namespace SmartMenza.API.Controllers
{
    [ApiController]
    [Route("api/NutritionGoalStatistics")]
    [Authorize]
    public class NutritionGoalStatisticsController : BaseAuthorizedController
    {
        private readonly INutritionGoalStatisticsService _service;

        public NutritionGoalStatisticsController(INutritionGoalStatisticsService service)
        {
            _service = service;
        }

        [HttpGet("today")]
        public async Task<IActionResult> GetTodayProgress()
        {
            var userId = GetUserId();
            var result = await _service.GetTodayProgressAsync(userId);
            return Ok(result);
        }
    }
}
