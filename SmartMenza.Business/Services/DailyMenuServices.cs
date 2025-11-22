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

            var dailyMenu = await _context.DailyMenus
                .Include(dm => dm.dish)
                .Where(dm => dm.date == today)
                .Select(dm => new
                {
                    dm.dishId,
                    dm.date,
                    Jelo = new
                    {
                        dm.dish.dishId,
                        dm.dish.title,
                        dm.dish.price,
                        dm.dish.description,
                        dm.dish.calories,
                        dm.dish.protein,
                        dm.dish.fat,
                        dm.dish.carbohydrates,
                        dm.dish.imgPath
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

            var menu = await _context.DailyMenus
                .Include(dm => dm.dish)
                .Where(dm => dm.date == parsedDate)
                .Select(dm => new
                {
                    dm.dishId,
                    dm.date,
                    Jelo = new
                    {
                        dm.dish.dishId,
                        dm.dish.title,
                        dm.dish.price,
                        dm.dish.description,
                        dm.dish.calories,
                        dm.dish.protein,
                        dm.dish.fat,
                        dm.dish.carbohydrates,
                        dm.dish.imgPath
                    }
                })
                .ToListAsync();

            return menu;
        }

    }
}
