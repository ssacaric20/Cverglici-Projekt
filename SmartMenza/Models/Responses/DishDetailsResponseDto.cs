namespace SmartMenza.API.Models.Responses
{
    public class DishDetailsResponseDto
    {
        public int DishId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Calories { get; set; }
        public decimal Protein { get; set; }
        public decimal Fat { get; set; }
        public decimal Carbohydrates { get; set; }
        public string? ImgPath { get; set; }

        // Za tagove sastojaka
        public List<string> Ingredients { get; set; } = new();

        // Za prosječnu ocjenu jela
        public double? AverageRating { get; set; }

        // Koliko ukupno ocjena postoji
        public int RatingsCount { get; set; }
    }
}

