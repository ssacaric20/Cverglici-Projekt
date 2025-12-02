using Microsoft.AspNetCore.Mvc;
using SmartMenza.Business.Services;
using SmartMenza.Business.Models.DailyMenu;

namespace SmartMenza.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DailyMenuController : ControllerBase
    {
        private readonly DailyMenuServices _dailyMenuServices;

        public DailyMenuController(DailyMenuServices dailyMenuServices)
        {
            _dailyMenuServices = dailyMenuServices;
        }

        [HttpGet("today")]
        public async Task<ActionResult<IEnumerable<DailyMenuListItemResponse>>> GetTodaysMenuAsync()
        {
            try
            {
                var dailyMenu = await _dailyMenuServices.GetTodaysMenuAsync();
                return Ok(dailyMenu);
            } catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error occured while fetching the menu!",
                    error = ex.Message
                });
            }
        }

        [HttpGet("date")]
        public async Task<ActionResult<IEnumerable<DailyMenuListItemResponse>>> GetMenuForDateAsync([FromQuery] string date)
        {
            try
            {
                var menu = await _dailyMenuServices.GetMenuForDateAsync(date);

                if (menu == null)
                {
                    return BadRequest(new { message = "Wrong date format! Use YYYY-MM-DD" });
                }

                return Ok(menu);
            } catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error occured while fetching the menu!",
                    error = ex.Message
                });
            }
        }
    }
}