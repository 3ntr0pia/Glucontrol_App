using DiabetesNoteBook.Domain.Models;
using DiabetesNoteBook.Infrastructure.Interfaces;

namespace DiabetesNoteBook.Infrastructure.Repositories
{
    public class SaveUsuarioMedicacionRepository : INewUsuarioMedicacion
    {
        private readonly DiabetesNoteBookContext _context;

        public SaveUsuarioMedicacionRepository(DiabetesNoteBookContext context)
        {
            _context = context;
        }

        public async Task SaveNuevaUsuarioMedicacion(UsuarioMedicacion newusuariomedicacion)
        {
            await _context.UsuarioMedicacions.AddAsync(newusuariomedicacion);
            await _context.SaveChangesAsync();
        }
    }
}
