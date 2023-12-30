using CapaAplicacion.DTOs;

namespace CapaAplicacion.Interfaces
{
    public interface INuevaPersonaService
    {
        Task ResgitroPersona(DTOPersonaRegistro request);
    }
}
