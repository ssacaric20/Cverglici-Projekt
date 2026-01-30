using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMenza.Business.Models.Statistics
{
    public class DishStatisticsResponse
    {
        public int DishId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? ImgPath { get; set; }
        public double? AverageRating { get; set; }
        public int RatingsCount { get; set; }
        public int FavoriteCount { get; set; }
    }
}
