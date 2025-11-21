using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartMenza.API.Data;
using SmartMenza.API.Models;

namespace SmartMenza.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DnevniMeniController : ControllerBase
    {
        private readonly AppDBContext _context;

        public DnevniMeniController(AppDBContext context)
        {
            _context = context;
        }

        [HttpGet("danas")]
        public async Task<ActionResult<IEnumerable<object>>> GetDanasnjiMeni()
        {
            try
            {
                var danas = DateOnly.FromDateTime(DateTime.Now);

                var dnevniMeni = await _context.DnevniMeniji
                    .Include(dm => dm.Jelo)
                    .Where(dm => dm.Datum == danas)
                    .Select(dm => new
                    {
                        dm.Id,
                        dm.Datum,
                        Jelo = new
                        {
                            dm.Jelo.Id,
                            dm.Jelo.Naziv,
                            dm.Jelo.Cijena,
                            dm.Jelo.Opis,
                            dm.Jelo.Kalorije,
                            dm.Jelo.Proteini,
                            dm.Jelo.Masti,
                            dm.Jelo.Ugljikohidrati,
                            dm.Jelo.SlikaPutanja
                        }
                    })
                    .ToListAsync();

                return Ok(dnevniMeni);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Greška prilikom dohvaćanja menija.", error = ex.Message });
            }
        }

        [HttpGet("datum")]
        public async Task<ActionResult<IEnumerable<object>>> GetMeniZaDatum([FromQuery] string datum)
        {
            try
            {
                if (!DateOnly.TryParse(datum, out DateOnly parsedDatum))
                {
                    return BadRequest(new { message = "Neispravan format datuma. Koristite format: YYYY-MM-DD" });
                }

                var dnevniMeni = await _context.DnevniMeniji
                    .Include(dm => dm.Jelo)
                    .Where(dm => dm.Datum == parsedDatum)
                    .Select(dm => new
                    {
                        dm.Id,
                        dm.Datum,
                        Jelo = new
                        {
                            dm.Jelo.Id,
                            dm.Jelo.Naziv,
                            dm.Jelo.Cijena,
                            dm.Jelo.Opis,
                            dm.Jelo.Kalorije,
                            dm.Jelo.Proteini,
                            dm.Jelo.Masti,
                            dm.Jelo.Ugljikohidrati,
                            dm.Jelo.SlikaPutanja
                        }
                    })
                    .ToListAsync();

                return Ok(dnevniMeni);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Greška prilikom dohvaćanja menija.", error = ex.Message });
            }
        }
    }
}