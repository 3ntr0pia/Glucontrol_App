using DiabetesNoteBook.Domain.Models;

namespace DiabetesNoteBook.Infrastructure.Interfaces
{
    public interface INewStringGuid
    {
        Task SaveNewStringGuid(Usuario operation);
    }
}
