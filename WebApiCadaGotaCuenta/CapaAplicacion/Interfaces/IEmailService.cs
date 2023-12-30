
using CapaAplicacion.DTOs;

namespace CapaAplicacion.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(DTOEmail request);
    }
}
