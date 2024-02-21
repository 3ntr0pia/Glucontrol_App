using DiabetesNoteBook.Domain.Models;
using DiabetesNoteBook.Infrastructure.Interfaces;

namespace DiabetesNoteBook.Infrastructure.Repositories
{
    public class NewMedicationRepository : INewMedicacion
    {
        private readonly DiabetesNoteBookContext _context;

        public NewMedicationRepository(DiabetesNoteBookContext context)
        {
            _context = context;
        }

        public async Task SaveNewMedication(Medicacione newMedicacione)
        {
            await _context.Medicaciones.AddAsync(newMedicacione);
            await _context.SaveChangesAsync();
        }
    }
}
