using DiabetesNoteBook.Domain.Models;
using DiabetesNoteBook.Infrastructure.Interfaces;

namespace DiabetesNoteBook.Infrastructure.Repositories
{
    public class ChangeUserData : IChangeUserData
    {
        private readonly DiabetesNoteBookContext _context;

        public ChangeUserData(DiabetesNoteBookContext context)
        {
            _context = context;
        }

        public async Task SaveChangeUserData(Usuario operation)
        {
            _context.Usuarios.Update(operation);
            await _context.SaveChangesAsync();
        }
    }
}
