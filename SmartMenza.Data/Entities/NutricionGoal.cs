namespace SmartMenza.Data.Entities
{
    public class NutricionGoal
    {
        // PK
        public int NutricionalGoalId { get; set; }
        public int CaloriesGoal { get; set; }
        public decimal ProteinsGoal{ get; set; }
        public decimal FatsGoal { get; set; }
        public decimal CarbohydratesGoal { get; set; }

        // u slucaju da ce se kasnije vodit evidencija ovisna o danima...
        public DateTime GoalSetDate { get; set; } 

        // FK
        public int UserId { get; set; }

        // nav
        public User User { get; set; } = null!;
    }
}
