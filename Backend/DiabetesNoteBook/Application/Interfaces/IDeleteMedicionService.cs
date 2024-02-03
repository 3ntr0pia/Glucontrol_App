using DiabetesNoteBook.Application.DTOs;

namespace DiabetesNoteBook.Application.Interfaces
{
    public interface IDeleteMedicionService
    {
        Task DeleteMedicion(DTOEliminarMedicion delete);
    }
}
