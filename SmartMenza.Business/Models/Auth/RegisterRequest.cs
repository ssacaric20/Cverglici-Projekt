// Ovo je samo ako se odlučimo da registracija uzima ime i prezime zasebno u frontend-u
namespace SmartMenza.Business.Models.Auth
{
    public sealed class RegisterRequest
    {
        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        // omogućuje biranje uloge (inače hardcode Student)
        public int? RoleId { get; set; }
    }
}
