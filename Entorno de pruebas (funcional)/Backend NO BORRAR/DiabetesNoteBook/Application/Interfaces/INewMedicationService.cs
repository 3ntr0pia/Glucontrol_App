using DiabetesNoteBook.Application.DTOs;

namespace DiabetesNoteBook.Application.Interfaces
{
    public interface INewMedicationService
    {
        Task NewRegister(DTOMedicacion userData);
    }
}
