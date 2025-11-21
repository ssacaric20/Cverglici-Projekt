namespace SmartMenza.Data.Models
{
    public class DishIngredientDto
    {
        // veza vise-vise jelo-sastojak
        // komponentni PK se definira u DbContextu, ovdje su samo FK
        // FK 1
        public int JeloId { get; set; }
        public DishDto Jelo { get; set; } = null!;

        // FK 2
        public int SastojakId { get; set; }
        public IngredientDto Sastojak { get; set; } = null!;
    }
}