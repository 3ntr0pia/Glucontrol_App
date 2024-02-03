using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace DiabetesNoteBook.Application.Validators
{
    public class PassValidation : ValidationAttribute
    {
        private readonly string nuevoPatron = @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";

        public PassValidation()
        {

        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string password = value as string;

            if (password == null)
            {
                return new ValidationResult($"La contraseña debe estar presente");
            }
            else
            {
                Regex regex = new Regex(nuevoPatron);

                if (regex.IsMatch(password))
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult($"El formato de la contraseña no es válido. Debe cumplir con el nuevo patrón.");
                }
            }
        }
    }
}
