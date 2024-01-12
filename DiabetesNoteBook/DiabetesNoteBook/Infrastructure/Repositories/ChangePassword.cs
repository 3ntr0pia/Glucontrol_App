using DiabetesNoteBook.Domain.Models;
using DiabetesNoteBook.Infrastructure.Interfaces;

namespace DiabetesNoteBook.Infrastructure.Repositories
{
    public class ChangePassword : IChangePassword
    {
        private readonly DiabetesNoteBookContext _context;

        public ChangePassword(DiabetesNoteBookContext context)
        {
            _context = context;
        }

        public async Task SaveNewPassword(Usuario operation)
        {
            _context.Usuarios.Update(operation);
            await _context.SaveChangesAsync();
        }
    }
}
