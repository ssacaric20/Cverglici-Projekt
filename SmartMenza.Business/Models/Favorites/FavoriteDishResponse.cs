using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMenza.Business.Models.Favorites
{
    public class FavoriteDishResponse
    {
        public int DishId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int? Calories { get; set; }
        public string? ImgPath { get; set; }
    }
}
