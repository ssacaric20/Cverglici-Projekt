namespace SmartMenza.Data.Models
{
    public class DailyMenuDto
    {
        // PK
        public int dailyMenuId { get; set; }
        public DateOnly date { get; set; }

        // FK
        public int dishId { get; set; }

        // nav
        public DishDto dish { get; set; } = null!;
    }
}
