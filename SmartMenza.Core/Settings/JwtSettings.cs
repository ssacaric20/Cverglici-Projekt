namespace SmartMenza.Core.Settings
{
    public class JwtSettings
    {
        public string SecretKey { get; set; } = string.Empty;

        public int ExpirationDays { get; set; } = 7;
    }
}

