namespace SmartMenza.Data.Entities
{
    public class FavoriteDish
    {
        // veza vise-vise korisnik-jelo
        // FK 1: tko
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        // FK 2: fav
        public int DishId { get; set; }
        public Dish Dish { get; set; } = null!;
    }
}
