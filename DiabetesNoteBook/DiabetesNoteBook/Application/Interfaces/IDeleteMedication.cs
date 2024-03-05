using DiabetesNoteBook.Application.DTOs;

namespace DiabetesNoteBook.Application.Interfaces
{
    public interface IDeleteMedication
    {
        Task DeleteMedication(DTODeleteMedication userData);
    }
}
