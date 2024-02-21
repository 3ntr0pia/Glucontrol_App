using DiabetesNoteBook.Domain.Models;

namespace DiabetesNoteBook.Infrastructure.Interfaces
{
    public interface INewMedicacion
    {
        Task SaveNewMedication(Medicacione newMedicacione);
    }
}
