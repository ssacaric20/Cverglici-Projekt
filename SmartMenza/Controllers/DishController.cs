using Microsoft.AspNetCore.Mvc;
using SmartMenza.Business.Models.Dishes;
using SmartMenza.Business.Services;
using SmartMenza.Business.Services.Interfaces;


namespace SmartMenza.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DishController : ControllerBase
    {
        private readonly IDishService _dishServices;

        public DishController(IDishService dishServices)
        {
            _dishServices = dishServices;
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult<DishDetailsResponse>> GetDishDetails(int id)
        {
            try
            {
                DishDetailsResponse? dish = await _dishServices.GetDishDetailsAsync(id);

                if (dish == null)
                {
                    return NotFound(new { message = "Dish not found" });
                }

                return Ok(dish);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error occurred while fetching dish details.",
                    error = ex.Message
                });
            }
        }
    }
}
