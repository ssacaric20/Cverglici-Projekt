namespace SmartMenza.Business.Models.Auth
{
    public class LoginResponse
    {
        public int userId { get; set; }
        public string message { get; set; } = string.Empty;
        public string token { get; set; } = string.Empty;
        public int roleId { get; set; }
    }
}
