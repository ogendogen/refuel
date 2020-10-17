using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Database.Models
{
    public enum FuelType
    {
        [Description("Benzyna 95")]
        [Display(Name="Benzyna 95")]
        Petrol95,
        [Display(Name="Benzyna 98")]
        [Description("Benzyna 98")]
        Petrol98,
        [Display(Name="Diesel")]
        [Description("Diesel")]
        Diesel,
        [Display(Name="GazLPG")]
        [Description("GazLPG")]
        LPG,
        [Display(Name="Inne")]
        [Description("Inne")]
        Other
    }
}