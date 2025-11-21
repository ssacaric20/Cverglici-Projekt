using Google.Apis.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartMenza.Business.Services;
using SmartMenza.Data.Data;
using SmartMenza.Data.Models;

namespace SmartMenza.API.Controllers
{
    [Route("api/[controller]")] // URL  /api/Korisnici
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly UserServices _userServices;

        public UserController(UserServices userServices)
        {
            _userServices = userServices;
        }


        // 3. Action Method: GET all korisnici
        // HTTP GET requesti na /api/Korisnici
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsersAsync()
        {

            try
            {
                var users = await _userServices.GetUsersAsync();

                return Ok(users);

            } catch (Exception ex)
            {

                return StatusCode(500, "An error occurred while retrieving users.");
            }

        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginKorisnikAsync([FromBody] Data.Models.LoginRequest request)
        {

            try
            {
                var loginUser = await _userServices.LoginUserAsync(request);

                if (loginUser == null)
                    return Unauthorized(new { message = "Invalid Email or Password!" });

                return StatusCode(200, loginUser);
            } 

            catch (Exception ex) 
            {
                return StatusCode(500, "An error occurred during login.");
            }
        }

        [HttpPost("google-login")]
        public async Task<IActionResult> LoginGoogleAsync([FromBody] GoogleLoginRequest request)
        {

            try
            {
                var googleLogin = await _userServices.LoginGoogleAsync(request);

                if (googleLogin == null)
                    return Unauthorized(new { message = "Invalid Google token!" });

                return StatusCode(200, googleLogin);
            } 
            catch (Exception ex) 
            {
                return StatusCode(500, "An error occurred during Google login.");
            }

            
        }
    }
}