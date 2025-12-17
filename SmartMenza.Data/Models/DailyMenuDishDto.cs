namespace SmartMenza.Data.Models
{
    public class DailyMenuDishDto
    {
        // primary key (dailyMenuId + dishId)
        public int dailyMenuId { get; set; }
        public int dishId { get; set; }

        // nav
        public DailyMenuDto dailyMenu { get; set; } = null!;

        public DishDto dish { get; set; } = null!;
    }
}