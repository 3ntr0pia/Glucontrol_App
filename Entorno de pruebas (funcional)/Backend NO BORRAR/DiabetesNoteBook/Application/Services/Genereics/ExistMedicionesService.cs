using DiabetesNoteBook.Application.DTOs;
using DiabetesNoteBook.Domain.Models;
using DiabetesNoteBook.Infrastructure.Controllers;
using Microsoft.EntityFrameworkCore;

namespace DiabetesNoteBook.Application.Services.Genereics
{
    public class ExistMedicionesService
    {

        private readonly DiabetesNoteBookContext _context;
        private readonly ILogger<UsersController> _logger;

        public ExistMedicionesService(DiabetesNoteBookContext context, ILogger<UsersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Medicione>> MedicionesPorId(int id)
        {
            try
            {
                var mediciones = await _context.Mediciones.Where(m => m.IdUsuarioNavigation.Id == id).ToListAsync();

                return mediciones;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al procesar la consulta de mediciones por ID de usuario");
                throw new Exception("Error al procesar la solicitud");
            }
        }

        public async Task<IEnumerable<Medicione>> MedicionesPorUserId(int id)
        {
            try
            {
                var mediciones = await _context.Mediciones.Where(m => m.IdUsuario == id).ToListAsync();

                return mediciones;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al procesar la consulta de mediciones por ID de usuario");
                throw new Exception("Error al procesar la solicitud");
            }
        }




    }
}
