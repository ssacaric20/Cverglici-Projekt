namespace SmartMenza.Business.Models.Auth
{
    public class LoginResponse
    {
        public int UserId { get; set; }
        public string Message { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public int RoleId { get; set; }
    }
}
