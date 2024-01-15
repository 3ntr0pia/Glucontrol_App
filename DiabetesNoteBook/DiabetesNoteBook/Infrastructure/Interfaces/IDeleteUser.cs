using DiabetesNoteBook.Domain.Models;

namespace DiabetesNoteBook.Infrastructure.Interfaces
{
    public interface IDeleteUser
    {
        Task DeleteUserRepository(Usuario delete);
    }
}
