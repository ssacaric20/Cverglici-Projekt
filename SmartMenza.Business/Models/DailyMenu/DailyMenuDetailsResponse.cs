using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SmartMenza.Business.Models.DailyMenu
{
    public class DailyMenuDetailsResponse
    {
        public int DailyMenuId { get; set; }
        public DateOnly Date { get; set; }
        public string Category { get; set; } = string.Empty;
        public List<DailyMenuDishListItemResponse> Dishes { get; set; } = new List<DailyMenuDishListItemResponse>();
    }
}