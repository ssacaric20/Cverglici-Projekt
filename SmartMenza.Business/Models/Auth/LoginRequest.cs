using System.Text.Json.Serialization;

namespace SmartMenza.Business.Models.Auth
{
    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        
        [JsonPropertyName("passwordHash")]
        public string Password { get; set; } = string.Empty;
    }
}