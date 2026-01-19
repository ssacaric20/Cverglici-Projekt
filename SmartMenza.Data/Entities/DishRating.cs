namespace SmartMenza.Data.Entities
{
    public class DishRating
    {
        // PK
        public int DishRatingId { get; set; }
        public int Rating { get; set; }

        // FK
        public int DishId { get; set; }
        public Dish Dish { get; set; } = null!;
    }
}
