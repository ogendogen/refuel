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
        [StringLength(32, ErrorMessage = "Nazwa firmy może mieć maksymalnie 32 znaki!")]
        [Display(Name = "Firma")]
        public string Manufacturer { get; set; }
        [Required]
        [StringLength(32, ErrorMessage = "Nazwa modelu może mieć maksymalnie 32 znaki!")]
        [Display(Name = "Model")]
        public string Model { get; set; }
        [Required]
        [Display(Name = "Silnik")]
        public decimal Engine { get; set; }
        [Required]
        [Display(Name = "Konie mechaniczne")]
        public int HorsePower { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Opis")]
        public string Description { get; set; }
    }
}
