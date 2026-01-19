namespace SmartMenza.Business.Models.Auth
{
    public class RegistrationRequest
    {
        public string Name { get; set; } = string.Empty;     
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
