using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Database.Models
{
    public enum FuelType
    {
        [Display(Name="Benzyna 95")]
        Petrol95,
        [Display(Name="Benzyna 98")]
        Petrol98,
        [Display(Name="Diesel")]
        Diesel,
        [Display(Name="GazLPG")]
        LPG,
        [Display(Name="Inne")]
        Other
    }
}