using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMenza.Business.Models.Statistics
{
    public class TopDishesResponse
    {
        public List<DishStatisticsResponse> TopRatedDishes { get; set; } = new();
        public List<DishStatisticsResponse> MostFavoritedDishes { get; set; } = new();
    }
}
