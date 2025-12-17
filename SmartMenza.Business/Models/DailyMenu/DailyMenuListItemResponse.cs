namespace SmartMenza.Business.Models.DailyMenu
{
    public class DailyMenuListItemResponse
    {
        public int DishId { get; set; }
        public DateOnly Date { get; set; }
        public string Category { get; set; } = string.Empty; 
        public DailyMenuDishListItemResponse Jelo { get; set; } = null!;
    }
}