using DiabetesNoteBook.Domain.Models;
using DiabetesNoteBook.Infrastructure.Interfaces;

namespace DiabetesNoteBook.Infrastructure.Repositories
{
    public class ChangePasswordMail : IChangePasswordMail
    {
        private readonly DiabetesNoteBookContext _context;

        public ChangePasswordMail(DiabetesNoteBookContext context)
        {
            _context = context;
        }
        public async Task SaveNewPasswordMail(Usuario operation)
        {
            _context.Usuarios.Update(operation);
            await _context.SaveChangesAsync();
        }
    }
}
