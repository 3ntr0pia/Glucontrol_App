using DiabetesNoteBook.Domain.Models;
using DiabetesNoteBook.Infrastructure.Interfaces;

namespace DiabetesNoteBook.Infrastructure.Repositories
{
    public class DeleteUser : IDeleteUser
    {
        private readonly DiabetesNoteBookContext _context;

        public DeleteUser(DiabetesNoteBookContext context)
        {
            _context = context;
        }
        public async Task DeleteUserRepository(Usuario delete)
        {
            _context.Operaciones.RemoveRange(delete.Operaciones);
            _context.Personas.RemoveRange(delete.Personas);

            _context.Usuarios.Remove(delete);
            await _context.SaveChangesAsync();
        }
    }
}
