namespace SmartMenza.Business.Models
{
    public class SetNutritionGoalRequest
    {
        public int CaloriesGoal { get; set; }
        public decimal ProteinsGoal { get; set; }
        public decimal FatsGoal { get; set; }
        public decimal CarbohydratesGoal { get; set; }
    }
}
