namespace SmartMenza.API.Models
{
    public class Sastojak
    {
        public int Id { get; set; }
        public string Naziv { get; set; } = string.Empty;

        // nav
        public ICollection<JeloSastojak> JeloSastojci { get; set; } = new List<JeloSastojak>();
    }
}