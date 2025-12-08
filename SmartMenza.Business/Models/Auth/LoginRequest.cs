namespace SmartMenza.Business.Models.Auth
{
    public class LoginRequest
    {
        public string email { get; set; } = string.Empty;
        public string passwordHash { get; set; } = string.Empty;
    }
}