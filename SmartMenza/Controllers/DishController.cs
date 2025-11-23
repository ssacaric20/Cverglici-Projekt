using Microsoft.AspNetCore.Mvc;
using SmartMenza.API.Models.Responses;
using SmartMenza.Business.Services;
using SmartMenza.Data.Models;

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
        public async Task<ActionResult<DishDetailsResponseDto>> GetDishDetails(int id)
        {
            try
            {
                DishDto? dish = await _dishServices.GetDishWithDetailsAsync(id);

                if (dish == null)
                {
                    return NotFound(new { message = "Dish not found" });
                }

                var response = new DishDetailsResponseDto
                {
                    DishId = dish.dishId,
                    Title = dish.title,
                    Description = dish.description,
                    Price = dish.price,
                    Calories = dish.calories,
                    Protein = dish.protein,
                    Fat = dish.fat,
                    Carbohydrates = dish.carbohydrates,
                    ImgPath = dish.imgPath,
                    Ingredients = dish.dishIngredients
                        .Select(di => di.ingredient.name)
                        .ToList(),
                    AverageRating = dish.dishRatings.Any()
                        ? dish.dishRatings.Average(r => r.rating)
                        : (double?)null,
                    RatingsCount = dish.dishRatings.Count
                };

                return Ok(response);
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

