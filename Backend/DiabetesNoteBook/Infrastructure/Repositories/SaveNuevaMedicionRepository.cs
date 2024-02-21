using DiabetesNoteBook.Domain.Models;
using DiabetesNoteBook.Infrastructure.Interfaces;

namespace DiabetesNoteBook.Infrastructure.Repositories
{
    public class SaveNuevaMedicionRepository:ISaveNuevaMedicion
    {
        private readonly DiabetesNoteBookContext _context;

        public SaveNuevaMedicionRepository(DiabetesNoteBookContext context)
        {
            _context = context;
        }

        public  async Task SaveNuevaMecion(Medicione newmedicion)
        {
            await _context.Mediciones.AddAsync(newmedicion);
            await _context.SaveChangesAsync();
        }

        
    }
}
