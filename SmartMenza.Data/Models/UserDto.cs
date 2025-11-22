using Microsoft.Identity.Client;

namespace SmartMenza.Data.Models
{
    public class UserDto
    {
        public int userId { get; set; }
        public string email { get; set; } = string.Empty;
        public string passwordHash { get; set; } = string.Empty; // hash za lozinke
        public string firstName { get; set; } = string.Empty;
        public string lastName { get; set; } = string.Empty;

        // FK
        public int roleId { get; set; }

        // navigacija
        public RoleDto role { get; set; } = null!;

        public ICollection<NutricionGoalDto> nutricionGoals { get; set; } = new List<NutricionGoalDto>();
        public ICollection<DailyFoodIntakeDto> dailyFoodIntakes { get; set; } = new List<DailyFoodIntakeDto>();
        public ICollection<FavoriteDishDto> favoriteDishes { get; set; } = new List<FavoriteDishDto>();
    }

    public class LoginRequest()
    {
        public string email { get; set; }
        public string passwordHash { get; set; }
    }

    public class GoogleLoginRequest()
    {
        public string tokenId { get; set; }
    }
}
