using DiabetesNoteBook.Application.DTOs;

namespace DiabetesNoteBook.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsyncRegister(DTOEmail userDataRegister);
        Task SendEmailAsyncChangePassword(DTOEmail userData);
    }
}
