namespace SmartMenza.Data.Models
{
    public class DailyMenuDto
    {
        // PK
        public int Id { get; set; }
        public DateOnly Datum { get; set; }

        // FK
        public int JeloId { get; set; }

        // nav
        public DishDto Jelo { get; set; } = null!;
    }
}
