using Microsoft.AspNetCore.Mvc;
using SmartMenza.Business.Models.Dishes;
using SmartMenza.Business.Services;

namespace SmartMenza.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DishController : ControllerBase
    {
        private readonly DishServices _dishServices;

        public DishController(DishServices dishServices)
        {
            _dishServices = dishServices;
        }

        // GET: api/Dish/1
        [HttpGet("{id}")]
        public async Task<ActionResult<DishDetailsResponse>> GetDishDetails(int id)
        {
            try
            {
                //  koristimo novu metodu iz servisa
                DishDetailsResponse? dish = await _dishServices.GetDishDetailsAsync(id);

                if (dish == null)
                {
                    return NotFound(new { message = "Dish not found" });
                }

                //  servis već vraća DTO spreman za frontend
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
