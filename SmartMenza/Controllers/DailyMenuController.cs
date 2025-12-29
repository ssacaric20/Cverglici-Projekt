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

        [HttpGet("{id}")]
        public async Task<ActionResult<DailyMenuDetailsResponse>> GetDailyMenuById(int id)
        {
            try
            {
                var menu = await _dailyMenuServices.GetDailyMenuByIdAsync(id);
                if (menu == null)
                {
                    return NotFound(new { message = "Meni nije pronađen." });
                }

                return Ok(menu);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Greška prilikom dohvaćanja menija.",
                    error = ex.Message
                });
            }
        }

        [HttpPost]
        public async Task<ActionResult<DailyMenuDetailsResponse>> CreateDailyMenu([FromBody] CreateDailyMenuRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var createdMenu = await _dailyMenuServices.CreateDailyMenuAsync(request);
                if (createdMenu == null)
                {
                    return BadRequest(new { message = "Greška prilikom kreiranja menija. Provjerite da li meni za taj datum i kategoriju već postoji ili da li sva jela postoje." });
                }

                return CreatedAtAction(
                    nameof(GetDailyMenuById),
                    new { id = createdMenu.DailyMenuId },
                    createdMenu
                );
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Greška prilikom kreiranja menija.",
                    error = ex.Message
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<DailyMenuDetailsResponse>> UpdateDailyMenu(int id, [FromBody] UpdateDailyMenuRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var updatedMenu = await _dailyMenuServices.UpdateDailyMenuAsync(id, request);
                if (updatedMenu == null)
                {
                    return NotFound(new { message = "Meni nije pronađen ili podaci nisu validni." });
                }

                return Ok(updatedMenu);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Greška prilikom ažuriranja menija.",
                    error = ex.Message
                });
            }
        }


    }
}