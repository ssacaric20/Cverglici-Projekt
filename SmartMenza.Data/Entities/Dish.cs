namespace SmartMenza.Data.Entities
{
    public class Dish
    {
        public int DishId { get; set; }
        public string Title { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Description { get; set; } = string.Empty;
        public int Calories { get; set; }
        public decimal Protein { get; set; }
        public decimal Fat { get; set; }
        public decimal Carbohydrates { get; set; }
        public decimal Fiber { get; set; }
        public string? ImgPath { get; set; }

        // navigacija
        public ICollection<DishIngredient> DishIngredients { get; set; } = new List<DishIngredient>();
        public ICollection<FavoriteDish> FavoriteDishes { get; set; } = new List<FavoriteDish>();
        public ICollection<DishRating> DishRatings { get; set; } = new List<DishRating>();
        public ICollection<DailyFoodIntake> DailyFoodIntakes { get; set; } = new List<DailyFoodIntake>();

        // veza vise-vise 
        public ICollection<DailyMenuDish> DailyMenuDishes { get; set; } = new List<DailyMenuDish>();
    }
}