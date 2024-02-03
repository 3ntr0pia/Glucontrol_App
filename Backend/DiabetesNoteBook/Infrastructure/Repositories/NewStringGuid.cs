using DiabetesNoteBook.Domain.Models;
using DiabetesNoteBook.Infrastructure.Interfaces;

namespace DiabetesNoteBook.Infrastructure.Repositories
{
    public class NewStringGuid : INewStringGuid
    {
        private readonly DiabetesNoteBookContext _context;

        public NewStringGuid(DiabetesNoteBookContext context)
        {
            _context = context;
        }

        public async Task SaveNewStringGuid(Usuario operation)
        {
            _context.Usuarios.Update(operation);
            await _context.SaveChangesAsync();
        }
    }
}
