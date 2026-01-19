namespace SmartMenza.Data.Entities
{
    public class DailyMenu
    {
        public int DailyMenuId { get; set; }
        public DateOnly Date { get; set; }
        public int Category { get; set; }  // 1 = Lunch, 2 = Dinner

        public ICollection<DailyMenuDish> DailyMenuDishes { get; set; } = new List<DailyMenuDish>();
    }
}