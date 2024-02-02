using DiabetesNoteBook.Domain.Models;

namespace DiabetesNoteBook.Infrastructure.Interfaces
{
    public interface IChangeUserData
    {
        Task SaveChangeUserData(Usuario operation);
        Task SaveChangePersonData(Persona operation);
    }
}
