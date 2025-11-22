namespace SmartMenza.Data.Models
{
    public class NutricionGoalDto
    {
        // PK
        public int nutricionalGoalId { get; set; }
        public int caloriesGoal { get; set; }
        public decimal proteinsGoal{ get; set; }
        public decimal fatsGoal { get; set; }
        public decimal carbohydratesGoal { get; set; }

        // u slucaju da ce se kasnije vodit evidencija ovisna o danima...
        public DateTime goalSetDate { get; set; } 

        // FK
        public int userId { get; set; }

        // nav
        public UserDto user { get; set; } = null!;
    }
}
