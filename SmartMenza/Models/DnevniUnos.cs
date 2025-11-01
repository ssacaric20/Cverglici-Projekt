namespace SmartMenza.API.Models
{
    public class DnevniUnos
    {
        // PK
        public int Id { get; set; }
        public DateTime DatumUnosa { get; set; }

        // FK 1: tko
        public int KorisnikId { get; set; }
        public Korisnik Korisnik { get; set; } = null!;

        // FK 2: sto
        public int JeloId { get; set; }
        public Jelo Jelo { get; set; } = null!;
    }
}
