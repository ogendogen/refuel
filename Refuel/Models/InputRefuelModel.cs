using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Database.Models;

namespace Refuel.Models
{
    public class InputRefuelModel
    {
        [Required]
        [Display(Name = "Data tankowania")]
        public DateTime Date { get; set; }
        [Required]
        [Display(Name = "Kilometry")]
        public uint Kilometers { get; set; }
        [Required]
        [Display(Name = "Cena za litr")]
        public decimal PricePerLiter { get; set; }
        [Required]
        [Display(Name = "Zatankowane litry")]
        public decimal Liters { get; set; }
        [Required]
        [Display(Name = "Spalanie")]
        public decimal Combustion { get; set; }
        [Required]
        [Display(Name = "Typ paliwa")]
        public FuelType Fuel { get; set; }
        [Required]
        [Display(Name = "Cena końcowa")]
        public decimal TotalPrice { get; set; }
    }
}
