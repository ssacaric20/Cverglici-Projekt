using SmartMenza.Business.Models.Statistics;
using SmartMenza.Business.Services.Interfaces;
using SmartMenza.Data.Entities;
using SmartMenza.Data.Repositories.Interfaces;

namespace SmartMenza.Business.Services
{
    public sealed class StatisticsService : IStatisticsService
    {
        private readonly IStatisticsRepository _statistics;

        public StatisticsService(IStatisticsRepository statistics)
        {
            _statistics = statistics;
        }

        public async Task<TopDishesResponse> GetTopDishesAsync(int count = 10)
        {
            var topRated = await _statistics.GetTopRatedDishesAsync(count);
            var mostFavorited = await _statistics.GetMostFavoritedDishesAsync(count);

            return new TopDishesResponse
            {
                TopRatedDishes = topRated.Select(MapToDishStatistics).ToList(),
                MostFavoritedDishes = mostFavorited.Select(MapToDishStatistics).ToList()
            };
        }

        public async Task<DishStatisticsResponse?> GetDishStatisticsAsync(int dishId)
        {
            var dish = await _statistics.GetTopRatedDishesAsync(int.MaxValue);
            var targetDish = dish.FirstOrDefault(d => d.DishId == dishId);

            if (targetDish == null)
            {
                var allDishes = await _statistics.GetMostFavoritedDishesAsync(int.MaxValue);
                targetDish = allDishes.FirstOrDefault(d => d.DishId == dishId);
            }

            return targetDish != null ? MapToDishStatistics(targetDish) : null;
        }

        private DishStatisticsResponse MapToDishStatistics(Dish dish)
        {
            var ratingsCount = dish.DishRatings?.Count ?? 0;
            double? avgRating = ratingsCount > 0
                ? dish.DishRatings!.Average(r => (double)r.Rating)
                : null;

            return new DishStatisticsResponse
            {
                DishId = dish.DishId,
                Title = dish.Title,
                ImgPath = dish.ImgPath,
                AverageRating = avgRating,
                RatingsCount = ratingsCount,
                FavoriteCount = dish.FavoriteDishes?.Count ?? 0
            };
        }
    }
}