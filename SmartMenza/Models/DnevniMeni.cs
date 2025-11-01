namespace SmartMenza.API.Models
{
    public class DnevniMeni
    {
        // PK
        public int Id { get; set; }
        public DateOnly Datum { get; set; }

        // FK
        public int JeloId { get; set; }

        // nav
        public Jelo Jelo { get; set; } = null!;
    }
}
