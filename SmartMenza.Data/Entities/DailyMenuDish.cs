namespace SmartMenza.Data.Entities
{
    public class DailyMenuDish
    {
        // primary key (dailyMenuId + dishId)
        public int DailyMenuId { get; set; }
        public int DishId { get; set; }

        // nav
        public DailyMenu DailyMenu { get; set; } = null!;

        public Dish Dish { get; set; } = null!;
    }
}