using Microsoft.Identity.Client;

namespace SmartMenza.API.Models
{
    public class Korisnik
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string LozinkaHash { get; set; } = string.Empty; // hash za lozinke
        public string Ime { get; set; } = string.Empty;
        public string Prezime { get; set; } = string.Empty;

        // FK
        public int UlogaId { get; set; }

        // navigacija
        public Uloga Uloga { get; set; } = null!;

        public ICollection<Cilj> Ciljevi { get; set; } = new List<Cilj>();
        public ICollection<DnevniUnos> DnevniUnosi { get; set; } = new List<DnevniUnos>();
        public ICollection<Favorit> Favoriti { get; set; } = new List<Favorit>();
    }

    public class Prijava()
    {
        public string Email { get; set; }
        public string LozinkaHash { get; set; }
    }
}
