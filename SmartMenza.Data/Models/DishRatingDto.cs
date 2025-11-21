namespace SmartMenza.Data.Models
{
    public class DishRatingDto
    {
        // PK
        public int Id { get; set; }
        public int Ocjena { get; set; }

        // FK
        public int JeloId { get; set; }
        public DishDto Jelo { get; set; } = null!;
    }
}
