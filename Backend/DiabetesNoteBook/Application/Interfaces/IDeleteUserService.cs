using DiabetesNoteBook.Application.DTOs;

namespace DiabetesNoteBook.Application.Interfaces
{
    public interface IDeleteUserService
    {
        Task DeleteUser(DTODeleteUser delete);
    }
}
