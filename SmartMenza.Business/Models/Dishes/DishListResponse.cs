using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMenza.Business.Models.Dishes
{
    public class DishListResponse
    {
        public int DishId { get; set; }
        public string Title { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Description { get; set; } = string.Empty;
        public int Calories { get; set; }
        public string? ImgPath { get; set; }
    }
}
