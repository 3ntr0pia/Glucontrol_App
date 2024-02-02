using DiabetesNoteBook.Application.DTOs;

namespace DiabetesNoteBook.Application.Interfaces
{
    public interface IOperationsService
    {
        Task AddOperacion(DTOOperation operation);
    }
}
