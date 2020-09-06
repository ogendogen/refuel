using System.ComponentModel;

namespace Database.Models
{
    public enum FuelType
    {
        [Description("Benzyna 95")]
        Petrol95,
        [Description("Benzyna 98")]
        Petrol98,
        [Description("Diesel")]
        Diesel,
        [Description("GazLPG")]
        LPG,
        [Description("Inne")]
        Other
    }
}