using CapaAplicacion.Validators;

namespace CapaInfraestructura.DTOs
{
    public class DTOUsuarioRegistro
    {
        [UsuarioValidacion]
        public string UserName { get; set; }

        [EmailValidacion]
        public string Email { get; set; }
        [PasswordValidacion]
        public string Password { get; set; }

    }

}
