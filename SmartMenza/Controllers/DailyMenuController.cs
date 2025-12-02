using Microsoft.AspNetCore.Mvc;
using SmartMenza.Business.Services;
using SmartMenza.Business.Models.DailyMenu;
using SmartMenza.Core.Enums;

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

        // get danasnji menu (sve kategorije il filter)
        [HttpGet("today")]
        public async Task<ActionResult<IEnumerable<DailyMenuListItemResponse>>> GetTodaysMenuAsync([FromQuery] string? category = null)
        {
            try
            {
                MenuCategory? categoryEnum = null;

                if (!string.IsNullOrEmpty(category))
                {
                    if (category.ToLower() == "lunch")
                        categoryEnum = MenuCategory.Lunch;
                    else if (category.ToLower() == "dinner")
                        categoryEnum = MenuCategory.Dinner;
                    else
                        return BadRequest(new { message = "Invalid category! Use 'lunch' or 'dinner'" });
                }

                var dailyMenu = await _dailyMenuServices.GetTodaysMenuAsync(categoryEnum);
                return Ok(dailyMenu);
            } catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error occurred while fetching the menu!",
                    error = ex.Message
                });
            }
        }

        // get danasnji menu grupirano
        [HttpGet("today/grouped")]
        public async Task<ActionResult<MenusByCategory>> GetTodaysMenusGroupedAsync()
        {
            try
            {
                var menus = await _dailyMenuServices.GetTodaysMenusGroupedAsync();
                return Ok(menus);
            } catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error occurred while fetching the menu!",
                    error = ex.Message
                });
            }
        }

        // get menu za specifican datum (sve kategorije il filter prema rucak/vecera)
        [HttpGet("date")]
        public async Task<ActionResult<IEnumerable<DailyMenuListItemResponse>>> GetMenuForDateAsync(
            [FromQuery] string date,
            [FromQuery] string? category = null)
        {
            try
            {
                MenuCategory? categoryEnum = null;

                if (!string.IsNullOrEmpty(category))
                {
                    if (category.ToLower() == "lunch")
                        categoryEnum = MenuCategory.Lunch;
                    else if (category.ToLower() == "dinner")
                        categoryEnum = MenuCategory.Dinner;
                    else
                        return BadRequest(new { message = "Invalid category! Use 'lunch' or 'dinner'" });
                }

                var menu = await _dailyMenuServices.GetMenuForDateAsync(date, categoryEnum);

                if (menu == null)
                {
                    return BadRequest(new { message = "Wrong date format! Use YYYY-MM-DD" });
                }

                return Ok(menu);
            } catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error occurred while fetching the menu!",
                    error = ex.Message
                });
            }
        }
    }
}