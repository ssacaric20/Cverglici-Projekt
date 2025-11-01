namespace SmartMenza.API.Models
{
    public class OcjenaJela
    {
        // PK
        public int Id { get; set; }
        public int Ocjena { get; set; }

        // FK
        public int JeloId { get; set; }
        public Jelo Jelo { get; set; } = null!;
    }
}
