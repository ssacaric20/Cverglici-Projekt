using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SmartMenza.Business.Models.DailyMenu
{
    public class CreateDailyMenuRequest
    {
        [Required(ErrorMessage = "Datum je obavezan")]
        public string Date { get; set; } = string.Empty;

        [Required(ErrorMessage = "Kategorija je obavezna")]
        [Range(1, 2, ErrorMessage = "Kategorija mora biti 1 (Lunch) ili 2 (Dinner)")]
        public int Category { get; set; }

        [Required(ErrorMessage = "Lista jela je obavezna")]
        [MinLength(1, ErrorMessage = "Meni mora sadržavati barem jedno jelo")]
        public List<int> DishIds { get; set; } = new List<int>();
    }
}