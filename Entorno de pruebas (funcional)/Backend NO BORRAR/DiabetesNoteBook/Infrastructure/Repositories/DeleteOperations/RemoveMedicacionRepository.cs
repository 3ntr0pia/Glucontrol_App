using DiabetesNoteBook.Domain.Models;
using DiabetesNoteBook.Infrastructure.Interfaces;

namespace DiabetesNoteBook.Infrastructure.Repositories.DeleteOperations
{
    public class RemoveMedicacionRepository
    {
        private readonly DiabetesNoteBookContext _context;

        public RemoveMedicacionRepository(DiabetesNoteBookContext context)
        {
            _context = context;
        }
        public async Task RemoveMedicacion(Medicacione medicacion)
        {
            _context.Medicaciones.RemoveRange(medicacion);
            _context.SaveChanges();
        }
    }
}
