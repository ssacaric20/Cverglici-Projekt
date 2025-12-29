namespace SmartMenza.Business.Models.DailyMenu
{
    public class DailyMenuListItemResponse
    {
        public int DailyMenuId { get; set; } 
        public int DishId { get; set; }
        public DateOnly Date { get; set; }
        public string Category { get; set; }
        public DailyMenuDishListItemResponse Jelo { get; set; }
    }
}