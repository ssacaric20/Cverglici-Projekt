namespace SmartMenza.Data.Models
{
    public class DishIngredientDto
    {
        // FK 1
        public int dishId { get; set; }
        public DishDto dish { get; set; } = null!;

        // FK 2
        public int ingredientId { get; set; }
        public IngredientDto ingredient { get; set; } = null!;
    }
}