using CapaAplicacion.DTOs;

namespace CapaAplicacion.Interfaces
{
    public interface IConfirmarEmailService
    {
        Task ConfirmacionEmail(DTOComprobarRegistro request);
    }
}
