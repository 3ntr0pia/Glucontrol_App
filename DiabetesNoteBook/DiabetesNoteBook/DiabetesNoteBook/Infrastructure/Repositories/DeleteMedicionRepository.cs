using DiabetesNoteBook.Domain.Models;
using DiabetesNoteBook.Infrastructure.Interfaces;

namespace DiabetesNoteBook.Infrastructure.Repositories
{
    public class DeleteMedicionRepository : IDeleteMedicion
    {
        private readonly DiabetesNoteBookContext _context;

        public DeleteMedicionRepository(DiabetesNoteBookContext context)
        {
            _context = context;
        }
        public async Task DeleteMedicion(Medicione delete)
        {
            _context.Mediciones.Remove(delete);
            await _context.SaveChangesAsync();
        }
    }
}
