using DiabetesNoteBook.Domain.Models;
using DiabetesNoteBook.Infrastructure.Interfaces;

namespace DiabetesNoteBook.Infrastructure.Repositories
{
    public class UserDeregistration : IUserDeregistration
    {
        private readonly DiabetesNoteBookContext _context;

        public UserDeregistration(DiabetesNoteBookContext context)
        {
            _context = context;
        }
        public async Task UserDeregistrationSave(Usuario delete)
        {
            _context.Personas.RemoveRange(delete.Personas);

            _context.Usuarios.Remove(delete);
            await _context.SaveChangesAsync();
        }
    }
}
