namespace SmartMenza.Data.Models
{
    public class DishDto
    {
        public int dishId { get; set; }
        public string title { get; set; } = string.Empty;
        public decimal price { get; set; }
        public string description { get; set; } = string.Empty;
        public int calories { get; set; }
        public decimal protein { get; set; }
        public decimal fat { get; set; }
        public decimal carbohydrates { get; set; }
        public decimal fiber { get; set; }
        public string? imgPath { get; set; }

        // navigacija
        public ICollection<DishIngredientDto> dishIngredients { get; set; } = new List<DishIngredientDto>();
        public ICollection<FavoriteDishDto> favoriteDishes { get; set; } = new List<FavoriteDishDto>();
        public ICollection<DishRatingDto> dishRatings { get; set; } = new List<DishRatingDto>();
        public ICollection<DailyFoodIntakeDto> dailyFoodIntakes { get; set; } = new List<DailyFoodIntakeDto>();

        // veza vise-vise *NOVO
        public ICollection<DailyMenuDishDto> dailyMenuDishes { get; set; } = new List<DailyMenuDishDto>();
    }
}