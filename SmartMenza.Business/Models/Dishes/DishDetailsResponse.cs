namespace SmartMenza.Business.Models.Dishes
{
    public class DishDetailsResponse
    {
        public int DishId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Calories { get; set; }
        public decimal Protein { get; set; }
        public decimal Fat { get; set; }
        public decimal Carbohydrates { get; set; }

        public decimal Fiber { get; set; }
        public string? ImgPath { get; set; }

        public List<string> Ingredients { get; set; } = new();

        public double? AverageRating { get; set; }

        public int RatingsCount { get; set; }
    }
}

