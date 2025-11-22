namespace SmartMenza.Data.Models
{
    public class DishRatingDto
    {
        // PK
        public int dishRatingId { get; set; }
        public int rating { get; set; }

        // FK
        public int dishId { get; set; }
        public DishDto dish { get; set; } = null!;
    }
}
