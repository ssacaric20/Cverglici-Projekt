namespace SmartMenza.Data.Models
{
    public class DailyFoodIntakeDto
    {
        // PK
        public int Id { get; set; }
        public DateTime DatumUnosa { get; set; }

        // FK 1: tko
        public int KorisnikId { get; set; }
        public UserDto Korisnik { get; set; } = null!;

        // FK 2: sto
        public int JeloId { get; set; }
        public DishDto Jelo { get; set; } = null!;
    }
}
