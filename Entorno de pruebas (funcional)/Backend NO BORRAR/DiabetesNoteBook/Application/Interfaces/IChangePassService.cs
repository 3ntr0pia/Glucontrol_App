using DiabetesNoteBook.Application.DTOs;

namespace DiabetesNoteBook.Application.Interfaces
{
    public interface IChangePassService
    {
        Task ChangePassId(DTOCambioPassPorId userData);
    }
}
