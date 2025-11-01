namespace SmartMenza.API.Models
{
    public class Uloga
    {
        public int Id { get; set; }
        public string NazivUloge { get; set; } = string.Empty;

        // navigacija (za EF core)
        public ICollection<Korisnik> Korisnici { get; set; } = new List<Korisnik>();
    }
}
