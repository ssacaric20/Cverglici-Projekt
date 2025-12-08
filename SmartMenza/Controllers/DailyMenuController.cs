using Microsoft.AspNetCore.Mvc;
using SmartMenza.Business.Services;
using SmartMenza.Business.Models.DailyMenu;
using SmartMenza.Core.Enums;
using SmartMenza.Business.Services.Interfaces;


namespace SmartMenza.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DailyMenuController : ControllerBase
    {
        private readonly IDailyMenuService _dailyMenuServices;

        public DailyMenuController(IDailyMenuService dailyMenuServices)
        {
            _dailyMenuServices = dailyMenuServices;
        }

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

        [HttpGet("today/grouped")]
        public async Task<ActionResult<MenusByCategoryResponse>> GetTodaysMenusGroupedAsync()
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