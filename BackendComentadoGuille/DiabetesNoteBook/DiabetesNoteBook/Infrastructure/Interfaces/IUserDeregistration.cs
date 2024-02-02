using DiabetesNoteBook.Domain.Models;

namespace DiabetesNoteBook.Infrastructure.Interfaces
{
    public interface IUserDeregistration
    {
        Task UserDeregistrationSave(Usuario delete);
    }
}
