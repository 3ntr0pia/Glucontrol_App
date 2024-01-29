using DiabetesNoteBook.Domain.Models;
using DiabetesNoteBook.Infrastructure.Interfaces;

namespace DiabetesNoteBook.Infrastructure.Repositories
{
    public class DeleteUserRepository : IDeleteUser
    {
        private readonly DiabetesNoteBookContext _context;

        public DeleteUserRepository(DiabetesNoteBookContext context)
        {
            _context = context;
        }
        public async Task DeleteUser(Usuario delete)
        {

            _context.Personas.RemoveRange(delete.Personas);

            _context.Usuarios.Remove(delete);
            await _context.SaveChangesAsync();
        }
    }
}
