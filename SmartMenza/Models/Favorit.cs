namespace SmartMenza.API.Models
{
    public class Favorit
    {
        // veza vise-vise korisnik-jelo
        // FK 1: tko
        public int KorisnikId { get; set; }
        public Korisnik Korisnik { get; set; } = null!;

        // FK 2: fav
        public int JeloId { get; set; }
        public Jelo Jelo { get; set; } = null!;
    }
}
