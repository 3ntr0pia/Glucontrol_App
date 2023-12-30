using CapaAplicacion.DTOs;

namespace CapaAplicacion.Interfaces
{
    public interface INuevoUsuarioService
    {
        Task ResgitroUsuario(DTOUsuarioRegistro request);
    }
}
