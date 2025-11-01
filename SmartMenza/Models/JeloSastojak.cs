namespace SmartMenza.API.Models
{
    public class JeloSastojak
    {
        // veza vise-vise jelo-sastojak
        // komponentni PK se definira u DbContextu, ovdje su samo FK
        // FK 1
        public int JeloId { get; set; }
        public Jelo Jelo { get; set; } = null!;

        // FK 2
        public int SastojakId { get; set; }
        public Sastojak Sastojak { get; set; } = null!;
    }
}