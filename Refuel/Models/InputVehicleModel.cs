using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Refuel.Models
{
    public class InputVehicleModel
    {
        [Required]
        [StringLength(32, ErrorMessage = "Nazwa marki może mieć maksymalnie 32 znaki!")]
        [Display(Name = "Marka")]
        public string Manufacturer { get; set; }
        [Required]
        [StringLength(32, ErrorMessage = "Nazwa modelu może mieć maksymalnie 32 znaki!")]
        [Display(Name = "Model")]
        public string Model { get; set; }
        [Required]
        [Range(0.1, 100.0)]
        [Display(Name = "Silnik")]
        public decimal Engine { get; set; }
        [Required]
        [Range(1, 1000)]
        [Display(Name = "Konie mechaniczne")]
        public int Horsepower { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "Opis")]
        public string Description { get; set; }
    }
}
