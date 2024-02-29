using DiabetesNoteBook.Domain.Models;
using DiabetesNoteBook.Infrastructure.Controllers;
using Microsoft.EntityFrameworkCore;

namespace DiabetesNoteBook.Application.Services.Genereics
{
    public class ExistUsersService
    {
        private readonly DiabetesNoteBookContext _context;
        private readonly ILogger<UsersController> _logger;

        public ExistUsersService(DiabetesNoteBookContext context, ILogger<UsersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> IdExists(int id)
        {
            try
            {
                var usuarioDB = await _context.Usuarios.AsTracking().FirstOrDefaultAsync(x => x.Id == id);

                return usuarioDB != null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al procesar el registro");
                throw new Exception("Error al procesar la solicitud");
            }
        }

    }
}
