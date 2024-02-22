using DiabetesNoteBook.Application.DTOs;
using DiabetesNoteBook.Application.Interfaces;
using DiabetesNoteBook.Domain.Models;
using DiabetesNoteBook.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DiabetesNoteBook.Application.Services
{
    public class DeleteMedicionService : IDeleteMedicionService
    {
        private readonly DiabetesNoteBookContext _context;
        private readonly IDeleteMedicion _deleteMedicion;

        public DeleteMedicionService(DiabetesNoteBookContext context, IDeleteMedicion deleteMedicion)
        {
            _context = context;
            _deleteMedicion = deleteMedicion;
        }
        public async Task DeleteMedicion(DTOEliminarMedicion delete)
        {
            var deleteMedicion = await _context.Mediciones.FirstOrDefaultAsync(x => x.Id == delete.Id);

            await _deleteMedicion.DeleteMedicion(deleteMedicion);
        }
    }
}
