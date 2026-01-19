using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartMenza.Business.Models.Auth;
using SmartMenza.Business.Models.Users;
using SmartMenza.Business.Services.Interfaces;


namespace SmartMenza.API.Controllers
{
    [Route("api/[controller]")] 
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserService _userServices;

        public UserController(IUserService userServices)
        {
            _userServices = userServices;
        }


        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserListItemResponse>>> GetUsers()
        {
            try
            {
                var users = await _userServices.GetUsersAsync();
                return Ok(users); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error while fetching users.", error = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LoginUserAsync([FromBody] LoginRequest request)
        {
            try
            {
                var loginUser = await _userServices.LoginUserAsync(request);

                if (loginUser == null)
                    return Unauthorized(new { message = "Invalid Email or Password!" });

                return Ok(loginUser);
            } 

            catch (Exception ex) 
            {
                return StatusCode(500, new
                {
                    message = "Error occurred during login.",
                    error = ex.Message
                });
            }
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<LoginResponse>> Register([FromBody] RegistrationRequest request)
        {
            try
            {
                var response = await _userServices.RegisterUserAsync(request);

                if (response == null)
                {
                    return BadRequest(new
                    {
                        message = "Invalid registration data or email already in use."
                    });
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error occurred while registering user.",
                    error = ex.Message
                });
            }
        }

        [AllowAnonymous]
        [HttpPost("google-login")]
        public async Task<ActionResult<LoginResponse>> LoginGoogle([FromBody] GoogleLoginRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.TokenId))
            {
                return BadRequest(new { message = "Token is required." });
            }

            var result = await _userServices.LoginGoogleAsync(request);

            if (result == null)
            {
                return Unauthorized(new { message = "Invalid Google token." });
            }

            return Ok(result);
        }
    }
}