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
            _context.Mediciones.RemoveRange(delete.Mediciones);
            _context.UsuarioMedicacions.RemoveRange(delete.UsuarioMedicacions);
            _context.Operaciones.RemoveRange(delete.Operaciones);
            _context.Usuarios.Remove(delete);
            await _context.SaveChangesAsync();
        }
    }
}
