namespace SmartMenza.Data.Models
{
    public class DailyFoodIntakeDto
    {
        // PK
        public int dailyFoodIntakeId { get; set; }
        public DateTime date { get; set; }

        // FK 1: tko
        public int userId { get; set; }
        public UserDto user{ get; set; } = null!;

        // FK 2: sto
        public int dishId { get; set; }
        public DishDto dish { get; set; } = null!;
    }
}
