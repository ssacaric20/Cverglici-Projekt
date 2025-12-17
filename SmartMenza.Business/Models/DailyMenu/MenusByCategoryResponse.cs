
namespace SmartMenza.Business.Models.DailyMenu
{
    public class MenusByCategoryResponse
    {
        public List<DailyMenuListItemResponse> Lunch { get; set; } = new();
        public List<DailyMenuListItemResponse> Dinner { get; set; } = new();
    }
}
