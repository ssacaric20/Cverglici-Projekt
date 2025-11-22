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
        public string? imgPath { get; set; }

        // FK
        public int nutricionalValueId { get; set; }

        // navigacija
        public ICollection<DishIngredientDto> dishIngredients { get; set; } = new List<DishIngredientDto>();
        public ICollection<DailyMenuDto> dailyMenus { get; set; } = new List<DailyMenuDto>();
        public ICollection<FavoriteDishDto> favoriteDishes { get; set; } = new List<FavoriteDishDto>();
        public ICollection<DishRatingDto> dishRatings { get; set; } = new List<DishRatingDto>();
        public ICollection<DailyFoodIntakeDto> intakeAmounts { get; set; } = new List<DailyFoodIntakeDto>();
    }
}