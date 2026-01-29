using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartMenza.Business.Models.Dishes;
using SmartMenza.Business.Services.Interfaces;


namespace SmartMenza.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DishController : ControllerBase
    {
        private readonly IDishService _dishServices;
        private readonly IImageService _imageService;

        public DishController(IDishService dishServices, IImageService imageService)
        {
            _dishServices = dishServices;
            _imageService = imageService;
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

        [Authorize]
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

        [Authorize]
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

        [Authorize]
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

        [Authorize]
        [HttpPost("{id}/upload-image")]
        public async Task<IActionResult> UploadDishImage(int id, IFormFile image)
        {
            try
            {
                if (image == null || image.Length == 0)
                {
                    return BadRequest(new { message = "Slika nije odabrana." });
                }

                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                var extension = Path.GetExtension(image.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(extension))
                {
                    return BadRequest(new { message = "Nevažeći format slike. Dozvoljeni formati: JPG, PNG, GIF, WEBP" });
                }

                if (image.Length > 5 * 1024 * 1024)
                {
                    return BadRequest(new { message = "Slika je prevelika. Maksimalna veličina je 5MB." });
                }

                var dish = await _dishServices.GetDishDetailsAsync(id);
                if (dish == null)
                {
                    return NotFound(new { message = "Jelo nije pronađeno." });
                }

                using var stream = image.OpenReadStream();
                var imageUrl = await _imageService.UploadImageAsync(stream, image.FileName);

                var updateRequest = new UpdateDishRequest
                {
                    Title = dish.Title,
                    Price = dish.Price,
                    Description = dish.Description ?? string.Empty,
                    Calories = dish.Calories,
                    Protein = dish.Protein,
                    Fat = dish.Fat,
                    Carbohydrates = dish.Carbohydrates,
                    Fiber = dish.Fiber,
                    ImgPath = imageUrl
                };

                var updatedDish = await _dishServices.UpdateDishAsync(id, updateRequest);

                return Ok(new
                {
                    message = "Slika uspješno uploadovana.",
                    imageUrl = imageUrl,
                    dish = updatedDish
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Greška prilikom uploada slike.",
                    error = ex.Message
                });
            }
        }
    }
}
