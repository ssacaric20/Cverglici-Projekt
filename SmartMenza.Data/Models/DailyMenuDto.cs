namespace SmartMenza.Data.Models
{
    public class DailyMenuDto
    {
        // PK
        public int dailyMenuId { get; set; }
        public DateOnly date { get; set; }

        public ICollection<DailyMenuDishDto> dailyMenuDishes { get; set; } = new List<DailyMenuDishDto>();
    }
}
