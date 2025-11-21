namespace SmartMenza.Data.Models
{
    public class RoleDto
    {
        public int Id { get; set; }
        public string NazivUloge { get; set; } = string.Empty;

        // navigacija (za EF core)
        public ICollection<UserDto> Korisnici { get; set; } = new List<UserDto>();
    }
}
