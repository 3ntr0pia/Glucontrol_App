using DiabetesNoteBook.Application.Interfaces;
using DiabetesNoteBook.Domain.Models;
using DiabetesNoteBook.Infrastructure.Interfaces;

namespace DiabetesNoteBook.Infrastructure.Repositories
{

    public class ConfirmationRegisterRepository : IConfirmationRegisterRepository
    {
        private readonly DiabetesNoteBookContext _context;

        public ConfirmationRegisterRepository(DiabetesNoteBookContext context)
        {
            _context = context;
        }

        public async Task ConfirmationRegisterTrue(Usuario confirm)
        {
            _context.Usuarios.Update(confirm);
            await _context.SaveChangesAsync();

        }
    }
}
