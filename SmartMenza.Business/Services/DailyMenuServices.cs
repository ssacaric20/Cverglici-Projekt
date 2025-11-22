using Microsoft.EntityFrameworkCore;
using SmartMenza.Data.Data;
using SmartMenza.Data.Data;
using SmartMenza.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SmartMenza.Business.Services
{
    public class DailyMenuServices
    {
        private readonly AppDBContext _context;

        public DailyMenuServices(AppDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<object>> GetTodaysMenuAsync()
        {
            var today = DateOnly.FromDateTime(DateTime.Now);

            var dailyMenu = await _context.DnevniMeniji
                .Include(dm => dm.Jelo)
                .Where(dm => dm.Datum == today)
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

            return dailyMenu;
        }

        public async Task<IEnumerable<object?>> GetMenuForDateAsync(string date)
        {
            if (!DateOnly.TryParse(date, out DateOnly parsedDate))
            {
                return null;
            }

            var menu = await _context.DnevniMeniji
                .Include(m => m.Jelo)
                .Where(m => m.Datum == parsedDate)
                .Select(m => new
                {
                    m.Id,
                    m.Datum,
                    Jelo = new
                    {
                        m.Jelo.Id,
                        m.Jelo.Naziv,
                        m.Jelo.Cijena,
                        m.Jelo.Opis,
                        m.Jelo.Kalorije,
                        m.Jelo.Proteini,
                        m.Jelo.Masti,
                        m.Jelo.Ugljikohidrati,
                        m.Jelo.SlikaPutanja
                    }
                })
                .ToListAsync();

            return menu;
        }

    }
}
