using DiabetesNoteBook.Domain.Models;

namespace DiabetesNoteBook.Infrastructure.Interfaces
{
    public interface IAddOperation
    {
        Task SaveAddOpertion(Operacione operation);
    }
}
