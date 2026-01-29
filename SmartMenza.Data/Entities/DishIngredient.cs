namespace SmartMenza.Data.Entities
{
    public class DishIngredient
    {
        // FK 1
        public int DishId { get; set; }
        public Dish Dish { get; set; } = null!;

        // FK 2
        public int IngredientId { get; set; }
        public Ingredient Ingredient { get; set; } = null!;
    }
}