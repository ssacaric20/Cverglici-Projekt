namespace SmartMenza.Data.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty; // hash za lozinke
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        // FK
        public int RoleId { get; set; }

        // navigacija
        public Role Role { get; set; } = null!;
        public ICollection<NutricionGoal> NutricionGoals { get; set; } = new List<NutricionGoal>();
        public ICollection<DailyFoodIntake> DailyFoodIntakes { get; set; } = new List<DailyFoodIntake>();
        public ICollection<FavoriteDish> FavoriteDishes { get; set; } = new List<FavoriteDish>();
        public ICollection<DishRating> DishRatings { get; set; } = new List<DishRating>();
    }
}