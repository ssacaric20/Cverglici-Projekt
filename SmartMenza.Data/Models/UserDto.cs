using Microsoft.Identity.Client;

namespace SmartMenza.Data.Models
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string LozinkaHash { get; set; } = string.Empty; // hash za lozinke
        public string Ime { get; set; } = string.Empty;
        public string Prezime { get; set; } = string.Empty;

        // FK
        public int UlogaId { get; set; }

        // navigacija
        public RoleDto Uloga { get; set; } = null!;

        public ICollection<NutricionGoalDto> Ciljevi { get; set; } = new List<NutricionGoalDto>();
        public ICollection<DailyFoodIntakeDto> DnevniUnosi { get; set; } = new List<DailyFoodIntakeDto>();
        public ICollection<FavoriteDishDto> Favoriti { get; set; } = new List<FavoriteDishDto>();
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
