namespace SmartMenza.Data.Models
{
    public class DailyMenuDto
    {
        public int dailyMenuId { get; set; }
        public DateOnly date { get; set; }
        public int category { get; set; }  // 1 = Lunch, 2 = Dinner

        public ICollection<DailyMenuDishDto> dailyMenuDishes { get; set; } = new List<DailyMenuDishDto>();
    }
}