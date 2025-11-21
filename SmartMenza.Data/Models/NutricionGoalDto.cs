namespace SmartMenza.Data.Models
{
    public class NutricionGoalDto
    {
        // PK
        public int Id { get; set; }
        public int KalorijeCilj { get; set; }
        public decimal ProteiniCilj { get; set; }
        public decimal MastiCilj { get; set; }
        public decimal UgljikohidratiCilj { get; set; }

        // u slucaju da ce se kasnije vodit evidencija ovisna o danima...
        public DateTime DatumPostavljanja { get; set; } 

        // FK
        public int KorisnikId { get; set; }

        // nav
        public UserDto Korisnik { get; set; } = null!;
    }
}
