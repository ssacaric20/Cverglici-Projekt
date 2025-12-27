using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace SmartMenza.Business.Models.Dishes
{
    public class CreateDishRequest
    {
        [Required(ErrorMessage = "Naziv jela je obavezan")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Naziv mora biti između 3 i 100 znakova")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Cijena je obavezna")]
        [Range(0.01, 1000, ErrorMessage = "Cijena mora biti između 0.01 i 1000")]
        public decimal Price { get; set; }

        [StringLength(500, ErrorMessage = "Opis može imati maksimalno 500 znakova")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Kalorije su obavezne")]
        [Range(0, 10000, ErrorMessage = "Kalorije moraju biti između 0 i 10000")]
        public int Calories { get; set; }

        [Required(ErrorMessage = "Proteini su obavezni")]
        [Range(0, 1000, ErrorMessage = "Proteini moraju biti između 0 i 1000")]
        public decimal Protein { get; set; }

        [Required(ErrorMessage = "Masti su obavezne")]
        [Range(0, 1000, ErrorMessage = "Masti moraju biti između 0 i 1000")]
        public decimal Fat { get; set; }

        [Required(ErrorMessage = "Ugljikohidrati su obavezni")]
        [Range(0, 1000, ErrorMessage = "Ugljikohidrati moraju biti između 0 i 1000")]
        public decimal Carbohydrates { get; set; }

        [Required(ErrorMessage = "Vlakna su obavezna")]
        [Range(0, 1000, ErrorMessage = "Vlakna moraju biti između 0 i 1000")]
        public decimal Fiber { get; set; }

        [StringLength(500, ErrorMessage = "Putanja slike može imati maksimalno 500 znakova")]
        public string? ImgPath { get; set; }
    }
}
