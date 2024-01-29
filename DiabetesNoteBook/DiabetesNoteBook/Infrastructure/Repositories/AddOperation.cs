using DiabetesNoteBook.Domain.Models;
using DiabetesNoteBook.Infrastructure.Interfaces;

namespace DiabetesNoteBook.Infrastructure.Repositories
{
    public class AddOperation : IAddOperation
    {
        private readonly DiabetesNoteBookContext _context;

        public AddOperation(DiabetesNoteBookContext context)
        {
            _context = context;
        }

        public async Task SaveAddOpertion(Operacione operation)
        {
            await _context.Operaciones.AddAsync(operation);
            await _context.SaveChangesAsync();
        }
    }
}
