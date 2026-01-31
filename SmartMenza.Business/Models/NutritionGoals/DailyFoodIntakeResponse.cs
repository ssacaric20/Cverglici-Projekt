namespace SmartMenza.Business.Models
{
    public class DailyFoodIntakeResponse
    {
        public int DailyFoodIntakeId { get; set; }
        public DateTime Date { get; set; }

        public int DishId { get; set; }
        public string DishTitle { get; set; } = string.Empty;

        public int Calories { get; set; }
        public decimal Protein { get; set; }
        public decimal Fat { get; set; }
        public decimal Carbohydrates { get; set; }

        public string? ImgPath { get; set; }
    }
}
