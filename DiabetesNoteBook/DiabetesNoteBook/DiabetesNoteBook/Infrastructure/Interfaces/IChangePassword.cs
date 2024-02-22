using DiabetesNoteBook.Domain.Models;

namespace DiabetesNoteBook.Infrastructure.Interfaces
{
    public interface IChangePassword
    {
        Task SaveNewPassword(Usuario operation);
    }
}
