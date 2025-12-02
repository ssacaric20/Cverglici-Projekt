namespace SmartMenza.Business.Models.DailyMenu
{
    public class DailyMenuDishListItemResponse
    {
        public int DishId { get; set; }
        public string Title { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Description { get; set; } = string.Empty;
        public int Calories { get; set; }
        public decimal Protein { get; set; }
        public decimal Fat { get; set; }
        public decimal Carbohydrates { get; set; }
        public string? ImgPath { get; set; }
    }

    public class DailyMenuListItemResponse
    {
        public int DishId { get; set; }
        public DateOnly Date { get; set; }
        public string Category { get; set; } = string.Empty;  // NOVO: "Lunch" or "Dinner"
        public DailyMenuDishListItemResponse Jelo { get; set; } = null!;
    }
}