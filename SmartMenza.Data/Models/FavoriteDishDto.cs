namespace SmartMenza.Data.Models
{
    public class FavoriteDishDto
    {
        // veza vise-vise korisnik-jelo
        // FK 1: tko
        public int userId { get; set; }
        public UserDto user { get; set; } = null!;

        // FK 2: fav
        public int dishId { get; set; }
        public DishDto dish { get; set; } = null!;
    }
}
