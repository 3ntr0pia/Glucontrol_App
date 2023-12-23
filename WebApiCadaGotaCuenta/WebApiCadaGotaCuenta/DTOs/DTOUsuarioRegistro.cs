using WebApiCadaGotaCuenta.Validators;

namespace WebApiCadaGotaCuenta.DTOs
{
    public class DTOUsuarioRegistro
    {
        public string UserName { get; set; }

        [EmailValidacion]
        public string Email { get; set; }

        public string Password { get; set; }

    }

}
