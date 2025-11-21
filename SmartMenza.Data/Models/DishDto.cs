namespace SmartMenza.Data.Models
{
    public class DishDto
    {
        public int Id { get; set; }
        public string Naziv { get; set; } = string.Empty;
        public decimal Cijena { get; set; }
        public string Opis { get; set; } = string.Empty;
        public int Kalorije { get; set; }
        public decimal Proteini { get; set; }
        public decimal Masti { get; set; }
        public decimal Ugljikohidrati { get; set; }
        public string? SlikaPutanja { get; set; }

        // FK
        public int NutritivneVrijednostiId { get; set; }

        // navigacija
        public ICollection<DishIngredientDto> JeloSastojci { get; set; } = new List<DishIngredientDto>();
        public ICollection<DailyMenuDto> DnevniMeniji { get; set; } = new List<DailyMenuDto>();
        public ICollection<FavoriteDishDto> FavoritanoOdKorisnika { get; set; } = new List<FavoriteDishDto>();
        public ICollection<DishRatingDto> Ocjene { get; set; } = new List<DishRatingDto>();
        public ICollection<DailyFoodIntakeDto> KonzumiraniUnosi { get; set; } = new List<DailyFoodIntakeDto>();
    }
}