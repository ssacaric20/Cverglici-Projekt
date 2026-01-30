namespace SmartMenza.Data.Entities
{
    public class DailyFoodIntake
    {
        // PK
        public int DailyFoodIntakeId { get; set; }
        public DateTime Date { get; set; }

        // FK 1: tko
        public int UserId { get; set; }
        public User User{ get; set; } = null!;

        // FK 2: sto
        public int DishId { get; set; }
        public Dish Dish { get; set; } = null!;
    }
}
