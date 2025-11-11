using Google.Apis.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartMenza.API.Data; 
using SmartMenza.API.Models;

namespace SmartMenza.API.Controllers
{
    [Route("api/[controller]")] // URL  /api/Korisnici
    [ApiController]
    public class KorisniciController : ControllerBase
    {
        // 1. db kontekst
        private readonly AppDBContext _context;

        // 2. konstruktoir za dependency injection
        public KorisniciController(AppDBContext context)
        {
            _context = context;
        }

        // 3. Action Method: GET all korisnici
        // HTTP GET requesti na /api/Korisnici
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Korisnik>>> GetKorisnici()
        {
            // EF za query 
            // ToListAsync() je dio LINQ
            var users = await _context.Korisnici.ToListAsync();

            return Ok(users);
        }

        // 4. Action Method: POST login korisnik
        // HTTP POST request na /api/korisnici/prijava
        [HttpPost("login")]
        public async Task<IActionResult> LoginKorisnik([FromBody] Prijava request)
        {
            bool formIsEmpty = IsLoginInputEmpty(request); 

            if (formIsEmpty) // Checks for empty fields 
            {
                return Unauthorized(new {message = "Invalid Email or Password!"}); // Sends Status Code 401 with a message
            }

            try
            {
                var user = ValidateLoginInput(request); // Checks if email and password hash match with the ones in DB
                if(user == null)
                return Unauthorized(new { message = "Invalid Email or Password!" }); // Sends Status Code 401 with a message
                return StatusCode(200, user); // Sends Status Code 200 with users data, in other words you did it 🎊🎊🎊😎😎😎
            } 

            catch (Exception ex) 
            {
                return StatusCode(500, ex); // Big stinky occured
            }
        }

        // 5. Action Method: POST prijava Google oAuth 
        // HTTP POST request na /api/korisnici/prijava-google-oAuth
        [HttpPost("prijava-google-oAuth")]
        public async Task<IActionResult> LoginGoogle([FromBody] PrijavaGoogle request)
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                // GOOGLE CLIENT ID FALI!!!!!!!!!!!!
                Audience = new[] { "STAVITI GOOGLE CLIENT ID" }
            };
            try
            {
                // validacija tokena sa google cloudom
                var payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken, settings);

                // Mora bit async jer je paylaod async
                // Provjera dal postoji korisnik u bazi
                var user = await _context.Korisnici.FirstOrDefaultAsync(u => u.Email == payload.Email);

                // Provjera dal korisnik postoji
                if (user == null)
                {
                    // Kreira novog korisika ako ne postoji
                    user = CreateUserForGoogleRegistration(payload);

                    // Pohrana korisnika u bazu
                    _context.Korisnici.Add(user);
                    await _context.SaveChangesAsync();
                }

                return StatusCode(200, user);
            } 
            catch (Exception ex) 
            { 
                return Unauthorized(new { message = "Invalid Google token!" }); 
            }

            
        }

        private Korisnik CreateUserForGoogleRegistration(GoogleJsonWebSignature.Payload payload)
        {
            var user = new Korisnik
            {
                Email = payload.Email,
                Ime = payload.GivenName,
                Prezime = payload.FamilyName,
                LozinkaHash = "", // Not too sure this is safe. Should be tested
                UlogaId = 1 // ovisno o ulozi, trebalo bi dodat enumeraciju
            };

            return user;
        }

        // Empty feild check
        private bool IsLoginInputEmpty(Prijava request)
        {
            if (request.LozinkaHash == "" || request.Email == "")
            {
                return true;
            }
            return false;
        }

        // Returns a valid user or NULL
        private Korisnik ValidateLoginInput(Prijava request)
        {
            var user = _context.Korisnici.FirstOrDefault(u => u.Email == request.Email && u.LozinkaHash == request.LozinkaHash);
            return user;
        }

        // druge radnje ([HttpPost] za stvaranje korisnika)
    }
}