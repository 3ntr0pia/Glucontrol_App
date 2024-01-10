using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;


namespace DiabetesNoteBook.Application.Validators
{
    public class UserValidation : ValidationAttribute
    {

        private readonly string nuevoPatron = @"^[A-Za-z0-9_-]{6,18}$";

        public UserValidation()
        {

        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string usuario = value as string;

            if (usuario == null)
            {
                return new ValidationResult($"El usuario debe estar presente");
            }
            else
            {
                Regex regex = new Regex(nuevoPatron);

                if (regex.IsMatch(usuario))
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult($"El formato de usuario no es válido. Debe cumplir con el nuevo patrón.");
                }
            }
        }
    }
}
