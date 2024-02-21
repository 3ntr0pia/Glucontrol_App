using DiabetesNoteBook.Application.DTOs;

namespace DiabetesNoteBook.Application.Interfaces
{
    public interface IChangePassMail
    {
        Task ChangePassEnlaceMail(DTOUsuarioChangePasswordMailConEnlace userData);
    }
}
