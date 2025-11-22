namespace SmartMenza.Data.Models
{
    public class IngredientDto
    {
        public int ingredientId { get; set; }
        public string name { get; set; } = string.Empty;

        // nav
        public ICollection<DishIngredientDto> dishIngredients { get; set; } = new List<DishIngredientDto>();
    }
}