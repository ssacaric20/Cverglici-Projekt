namespace SmartMenza.Data.Entities
{
    public class DishRating
    {
        // PK
        public int DishRatingId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // FK
        public int DishId { get; set; }
        public Dish Dish { get; set; } = null!;

        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}