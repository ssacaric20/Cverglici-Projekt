namespace SmartMenza.Data.Models
{
    public class FavoriteDishDto
    {
        // veza vise-vise korisnik-jelo
        // FK 1: tko
        public int KorisnikId { get; set; }
        public UserDto Korisnik { get; set; } = null!;

        // FK 2: fav
        public int JeloId { get; set; }
        public DishDto Jelo { get; set; } = null!;
    }
}
