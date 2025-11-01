using Microsoft.AspNetCore.Http;
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

        // druge radnje ([HttpPost] za stvaranje korisnika)
    }
}