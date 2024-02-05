using DiabetesNoteBook.Application.DTOs;

namespace DiabetesNoteBook.Application.Interfaces
{
    public interface IConfirmEmailService
    {
        Task ConfirmEmail(DTOConfirmRegistrtion confirm);
    }
}
