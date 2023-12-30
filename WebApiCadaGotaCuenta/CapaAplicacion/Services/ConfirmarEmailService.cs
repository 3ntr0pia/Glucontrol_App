using CapaAplicacion.DTOs;
using CapaAplicacion.Interfaces;
using CapaDominio.Models;
using Microsoft.EntityFrameworkCore;

namespace CapaAplicacion.Services
{
    public class ConfirmarEmailService : IConfirmarEmailService
    {
        private readonly GlucoControlContext _context;
        private readonly OperacionesService _operacionesService;

        public ConfirmarEmailService(GlucoControlContext context, OperacionesService operacionesService)
        {
            _context = context;
            _operacionesService = operacionesService;
        }

        public async Task ConfirmacionEmail(DTOComprobarRegistro request)
        {

            var usuarioUpdate = _context.Usuarios.AsTracking().FirstOrDefault(x => x.Id == request.UserId);

            if (usuarioUpdate != null)
            {
                usuarioUpdate.ConfirmacionEmail = true;
                _context.Usuarios.Update(usuarioUpdate);
                await _context.SaveChangesAsync(); // Guarda los cambios en la base de datos
                await _operacionesService.AddOperacionRegister("ConfirmarEmailService", "ConfirmacionEmail", request.UserId);
            }

        }

    }
}
