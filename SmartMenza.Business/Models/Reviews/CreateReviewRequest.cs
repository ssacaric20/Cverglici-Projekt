using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SmartMenza.Business.Models.Reviews
{
    public class CreateReviewRequest
    {
        [Required(ErrorMessage = "Ocjena je obavezna.")]
        [Range(1, 5, ErrorMessage = "Ocjena mora biti između 1 i 5.")]
        public int Rating { get; set; }

        [MaxLength(500, ErrorMessage = "Komentar ne smije biti dulji od 500 znakova.")]
        public string Comment { get; set; } = string.Empty;
    }
}