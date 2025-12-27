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

        [HttpGet]
        public async Task<IActionResult> GetAllDishes()
        {
            try
            {
                var dishes = await _dishServices.GetAllDishesAsync();
                return Ok(dishes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Greška prilikom dohvaćanja jela.",
                    error = ex.Message
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateDish([FromBody] CreateDishRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var createdDish = await _dishServices.CreateDishAsync(request);

                if (createdDish == null)
                {
                    return StatusCode(500, new { message = "Greška prilikom kreiranja jela." });
                }

                return CreatedAtAction(
                    nameof(GetDishDetails),
                    new { id = createdDish.DishId },
                    createdDish
                );
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Greška prilikom kreiranja jela.",
                    error = ex.Message
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDish(int id, [FromBody] UpdateDishRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var updatedDish = await _dishServices.UpdateDishAsync(id, request);

                if (updatedDish == null)
                {
                    return NotFound(new { message = "Jelo nije pronađeno." });
                }

                return Ok(updatedDish);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Greška prilikom ažuriranja jela.",
                    error = ex.Message
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDish(int id)
        {
            try
            {
                var result = await _dishServices.DeleteDishAsync(id);

                if (!result)
                {
                    return NotFound(new { message = "Jelo nije pronađeno." });
                }

                return Ok(new { message = "Jelo uspješno obrisano." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Greška prilikom brisanja jela.",
                    error = ex.Message
                });
            }
        }

    }
}
