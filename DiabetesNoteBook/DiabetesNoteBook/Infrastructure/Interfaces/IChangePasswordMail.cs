using DiabetesNoteBook.Domain.Models;

namespace DiabetesNoteBook.Infrastructure.Interfaces
{
    public interface IChangePasswordMail
    {
        Task SaveNewPasswordMail(Usuario operation);
    }
}
