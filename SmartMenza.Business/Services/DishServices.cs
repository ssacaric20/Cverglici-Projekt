using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartMenza.Data.Data;
using SmartMenza.Data.Models;

namespace SmartMenza.Business.Services
{
    public class DishServices
    {
        private readonly AppDBContext _context;

        public DishServices(AppDBContext context)
        {
            _context = context;
        }

        // Dohvaća jelo zajedno sa sastojcima i ocjenama
        public async Task<DishDto?> GetDishWithDetailsAsync(int id)
        {
            var dish = await _context.Dishes
                .Include(d => d.dishIngredients)
                    .ThenInclude(di => di.ingredient)
                .Include(d => d.dishRatings)
                .FirstOrDefaultAsync(d => d.dishId == id);

            return dish;
        }
    }
}

