using DiabetesNoteBook.Application.DTOs;

namespace DiabetesNoteBook.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsyncRegister(DTOEmail userData);
        Task SendEmailAsyncChangePassword(DTOEmail userData);
    }
}
