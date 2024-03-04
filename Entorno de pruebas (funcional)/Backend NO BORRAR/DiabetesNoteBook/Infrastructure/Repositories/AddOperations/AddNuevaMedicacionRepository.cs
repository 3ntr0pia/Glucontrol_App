using DiabetesNoteBook.Domain.Models;
using DiabetesNoteBook.Infrastructure.Interfaces;

namespace DiabetesNoteBook.Infrastructure.Repositories.AddOperations
{
    public class AddNuevaMedicacionRepository : IAddNuevaMedicacion
    {
        private readonly DiabetesNoteBookContext _context;

        public AddNuevaMedicacionRepository(DiabetesNoteBookContext context)
        {
            _context = context;
        }
        public async Task AddNuevaMedicacion(Medicacione medicacion)
        {
            _context.Medicaciones.Add(medicacion);
            await _context.SaveChangesAsync();


        }
    }
}
