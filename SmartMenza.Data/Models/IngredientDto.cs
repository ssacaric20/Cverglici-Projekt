namespace SmartMenza.Data.Models
{
    public class IngredientDto
    {
        public int Id { get; set; }
        public string Naziv { get; set; } = string.Empty;

        // nav
        public ICollection<DishIngredientDto> JeloSastojci { get; set; } = new List<DishIngredientDto>();
    }
}