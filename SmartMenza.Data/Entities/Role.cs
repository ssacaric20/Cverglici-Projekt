namespace SmartMenza.Data.Entities
{
    public class Role
    {
        public int RoleId { get; set; }
        public string RoleTitle { get; set; } = string.Empty;

        // navigacija (za EF core)
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
