namespace SmartMenza.API.Models
{
    public class Jelo
    {
        public int Id { get; set; }
        public string Naziv { get; set; } = string.Empty;
        public decimal Cijena { get; set; }
        public string Opis { get; set; } = string.Empty;
        public int Kalorije { get; set; }
        public decimal Proteini { get; set; }
        public decimal Masti { get; set; }
        public decimal Ugljikohidrati { get; set; }
        public string? SlikaPutanja { get; set; }

        // FK
        public int NutritivneVrijednostiId { get; set; }

        // navigacija
        public ICollection<JeloSastojak> JeloSastojci { get; set; } = new List<JeloSastojak>();
        public ICollection<DnevniMeni> DnevniMeniji { get; set; } = new List<DnevniMeni>();
        public ICollection<Favorit> FavoritanoOdKorisnika { get; set; } = new List<Favorit>();
        public ICollection<OcjenaJela> Ocjene { get; set; } = new List<OcjenaJela>();
        public ICollection<DnevniUnos> KonzumiraniUnosi { get; set; } = new List<DnevniUnos>();
    }
}