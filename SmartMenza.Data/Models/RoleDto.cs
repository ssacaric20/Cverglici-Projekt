namespace SmartMenza.Data.Models
{
    public class RoleDto
    {
        public int roleId { get; set; }
        public string roleTitle { get; set; } = string.Empty;

        // navigacija (za EF core)
        public ICollection<UserDto> users { get; set; } = new List<UserDto>();
    }
}
