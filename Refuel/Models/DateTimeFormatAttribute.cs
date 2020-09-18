using System;
using System.ComponentModel.DataAnnotations;

namespace Refuel.Models
{
    public class DateTimeFormatAttribute : ValidationAttribute
    {
        public string Format { get; set; }
        public DateTimeFormatAttribute(string format)
        {
            Format = format;
        }

        public string GetIncorrectFormatErrorMessage() => $"Niepoprawny format daty";
        public string GetFutureDateFormatErrorMessage() => $"Data jest z przyszłości";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            InputRefuelModel refuelModel = (InputRefuelModel)validationContext.ObjectInstance;
            string dt = refuelModel.Date;

            bool result = DateTime.TryParseExact(dt, Format, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime refuelDateDt);
            
            if (!result)
            {
                return new ValidationResult(GetIncorrectFormatErrorMessage());
            }

            if (refuelDateDt > DateTime.Now)
            {
                return new ValidationResult(GetFutureDateFormatErrorMessage());
            }

            return ValidationResult.Success;
        }
    }
}