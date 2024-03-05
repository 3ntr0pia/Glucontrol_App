using DiabetesNoteBook.Application.DTOs;

namespace DiabetesNoteBook.Application.Interfaces
{
    public interface INewRegister
    {
        Task NewRegister(DTORegister userData);
    }
}
