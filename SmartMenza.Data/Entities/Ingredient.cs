namespace SmartMenza.Data.Entities
{
    public class Ingredient
    {
        public int IngredientId { get; set; }
        public string Name { get; set; } = string.Empty;

        // nav
        public ICollection<DishIngredient> DishIngredients { get; set; } = new List<DishIngredient>();
    }
}