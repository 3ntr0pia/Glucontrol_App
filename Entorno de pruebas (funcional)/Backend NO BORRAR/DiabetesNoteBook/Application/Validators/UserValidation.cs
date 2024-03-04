using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;


namespace DiabetesNoteBook.Application.Validators
{
    public class UserValidation : ValidationAttribute
    {
        //Establecemos un patron para el nombre de usuario

        private readonly string nuevoPatron = @"^[A-Za-z0-9_-]{6,18}$";
        //Creamos el constructor

        public UserValidation()
        {

        }
        //Metodo interno de ValidationAttribute

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {            
            //Comprobamos si el usuario es un string lo que recibe

            string usuario = value as string;
            //Si el campo de usuario esta vacio muestra este mensaje

            if (usuario == null)
            {
                return new ValidationResult($"El usuario debe estar presente");
            }
            else
            {
                //Si no esta vacio comprobamos si cumple con el patron si cumple sigue hacia delante

                Regex regex = new Regex(nuevoPatron);

                if (regex.IsMatch(usuario))
                {
                    return ValidationResult.Success;
                }
                else
                {
                    //Si no cumple con el patron muestra este mensaje

                    return new ValidationResult($"El formato de usuario no es válido. Debe cumplir con el nuevo patrón.");
                }
            }
        }
    }
}
