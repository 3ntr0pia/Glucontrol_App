using DiabetesNoteBook.Application.DTOs;

namespace DiabetesNoteBook.Application.Interfaces
{
    public interface IDeleteUserService
    {
        Task DeleteUserService(DTODeleteUser delete);
    }
}
