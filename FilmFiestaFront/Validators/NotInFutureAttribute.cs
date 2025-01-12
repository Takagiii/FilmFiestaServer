using System.ComponentModel.DataAnnotations;

namespace FilmFiestaFront.Validators
{
    public class NotInFutureAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is DateOnly date)
            {
                if (date > DateOnly.FromDateTime(DateTime.Now))
                {
                    return new ValidationResult("Release date cannot be in the future.");
                }
            }
            
            return ValidationResult.Success;
        }
    }
}
