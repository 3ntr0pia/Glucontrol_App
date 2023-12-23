using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace WebApiCadaGotaCuenta.Validators
{
    public class EmailValidacion : ValidationAttribute
    {
        private readonly string pattern = @"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$";

        public EmailValidacion()
        {
            
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string email = value as string;

            if (email == null)
            {
                return new ValidationResult($"El email debe estar presente");
            }
            else
            {
                Regex regex = new Regex(pattern);

                if (regex.IsMatch(email))
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult($"El formato del email no es válido");
                }
            }
        }
    }
}
