using DiabetesNoteBook.Domain.Models;
using DiabetesNoteBook.Infrastructure.Interfaces;

namespace DiabetesNoteBook.Infrastructure.Repositories
{
    public class DeleteUserMedication : IDeleteUserMedication
    {

        private readonly DiabetesNoteBookContext _context;

        public DeleteUserMedication(DiabetesNoteBookContext context)
        {
            _context = context;
        }

        public async Task DeleteUserMedications(UsuarioMedicacion userMedication)
        {
            _context.UsuarioMedicacions.Remove(userMedication);
            await _context.SaveChangesAsync();
        }
    }
}
