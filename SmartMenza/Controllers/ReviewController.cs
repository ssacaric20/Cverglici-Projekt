using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartMenza.Business.Models.Reviews;
using SmartMenza.Business.Services.Interfaces;
using System.Security.Claims;

namespace SmartMenza.API.Controllers
{
    [ApiController]
    [Route("api/dishes/{dishId}/reviews")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        /// <summary>
        /// Dohvaća sve recenzije za određeno jelo
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetReviewsByDish(int dishId)
        {
            try
            {
                var reviews = await _reviewService.GetReviewsByDishIdAsync(dishId);

                if (reviews == null || reviews.Count == 0)
                {
                    return Ok(new { message = "Nema recenzija za ovo jelo.", reviews = new List<DishReviewResponse>() });
                }

                return Ok(reviews);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Greška prilikom dohvaćanja recenzija.",
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// Dodaje novu recenziju za jelo
        /// </summary>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateReview(int dishId, [FromBody] CreateReviewRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized(new { message = "Korisnik nije autentificiran." });
                }

                var review = await _reviewService.CreateReviewAsync(userId, dishId, request);

                if (review == null)
                {
                    return BadRequest(new { message = "Nije moguće dodati recenziju. Jelo možda ne postoji ili ste već ostavili recenziju." });
                }

                return CreatedAtAction(
                    nameof(GetReviewsByDish),
                    new { dishId = dishId },
                    review
                );
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Greška prilikom dodavanja recenzije.",
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// Ažurira postojeću recenziju
        /// </summary>
        [Authorize]
        [HttpPut("{reviewId}")]
        public async Task<IActionResult> UpdateReview(int dishId, int reviewId, [FromBody] UpdateReviewRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized(new { message = "Korisnik nije autentificiran." });
                }

                var updatedReview = await _reviewService.UpdateReviewAsync(userId, reviewId, request);

                if (updatedReview == null)
                {
                    return NotFound(new { message = "Recenzija nije pronađena ili nemate pravo uređivati tuđu recenziju." });
                }

                return Ok(updatedReview);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Greška prilikom ažuriranja recenzije.",
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// Briše recenziju
        /// </summary>
        [Authorize]
        [HttpDelete("{reviewId}")]
        public async Task<IActionResult> DeleteReview(int dishId, int reviewId)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized(new { message = "Korisnik nije autentificiran." });
                }

                var result = await _reviewService.DeleteReviewAsync(userId, reviewId);

                if (!result)
                {
                    return NotFound(new { message = "Recenzija nije pronađena ili nemate pravo brisati tuđu recenziju." });
                }

                return Ok(new { message = "Recenzija uspješno obrisana." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Greška prilikom brisanja recenzije.",
                    error = ex.Message
                });
            }
        }
    }
}